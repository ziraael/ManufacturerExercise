import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { OrderService } from '../../services/order.service';
import * as signalR from '@microsoft/signalr';
import { OrderDTO } from '../../models/Order.dto';
import { BehaviorSubject, lastValueFrom, Subject } from 'rxjs';

@Component({
  selector: 'app-orders-dashboard',
  templateUrl: './orders-dashboard.component.html',
  styleUrls: ['./orders-dashboard.component.scss']
})

export class OrdersDashboardComponent implements OnInit {
  observable: Subject<any> = new Subject<any>();
  observable$ = this.observable.asObservable();
  private hasLoaded = false;
  orders$: BehaviorSubject<any[]> = new BehaviorSubject<any[]>([]);
  message: string = '';
  canCancelOrder: boolean = true;
  orderIdToModify: BehaviorSubject<string> = new BehaviorSubject<string>("");
  queueLoading: any[] = [];
  private engineHubConnection: signalR.HubConnection;
  private chassisHubConnection: signalR.HubConnection;
  private optionHubConnection: signalR.HubConnection;
  private orderHubConnection: signalR.HubConnection;
  private warehouseHubConnection: signalR.HubConnection;

  constructor(private orderService: OrderService, private router: Router, private cdr: ChangeDetectorRef) {
  }

  ngOnInit() {
    this.startConnections();

    this.observable$.subscribe(order => {
      if (!this.hasLoaded) {
        this.loadOrders().then(() => {
          this.updateOrder(order.id, true);
        });
      }
    });
  }

  async ngAfterViewInit(){
    if (!this.hasLoaded) {
      await this.loadOrders();
    }
  }

  updateOrder(orderId: string, newState: boolean) {
    const currentOrders = this.orders$.getValue();
    const orderIndex = currentOrders.findIndex(order => order.id === orderId);
    debugger
    if (orderIndex !== -1) {
      currentOrders[orderIndex].isEngineProduced = newState;
      this.orders$.next(currentOrders);
    }
  }

  public startConnections(): void {
    this.engineHubConnection = new signalR.HubConnectionBuilder()
      .withUrl('http://localhost:5372/engineHub')
      .withAutomaticReconnect()
      .build();

    this.chassisHubConnection = new signalR.HubConnectionBuilder()
      .withUrl('http://localhost:5207/chassisHub')
      .withAutomaticReconnect()
      .build();

    this.optionHubConnection = new signalR.HubConnectionBuilder()
      .withUrl('http://localhost:5134/optionHub')
      .withAutomaticReconnect()
      .build();

    this.orderHubConnection = new signalR.HubConnectionBuilder()
      .withUrl('http://localhost:5229/orderHub')
      .withAutomaticReconnect()
      .build();

    this.warehouseHubConnection = new signalR.HubConnectionBuilder()
      .withUrl('http://localhost:5297/warehouseHub')
      .withAutomaticReconnect()
      .build();

    this.engineHubConnection
      .start()
      .then(() => console.log('EngineHub connection started'))
      .catch(err => console.log('Error while starting EngineHub connection: ' + err));

    this.chassisHubConnection
      .start()
      .then(() => console.log('ChassisHub connection started'))
      .catch(err => console.log('Error while starting ChassisHub connection: ' + err));

    this.optionHubConnection
      .start()
      .then(() => console.log('OptionHub connection started'))
      .catch(err => console.log('Error while starting OptionHub connection: ' + err));

    this.orderHubConnection
      .start()
      .then(() => console.log('OrderHub connection started'))
      .catch(err => console.log('Error while starting OrderHub connection: ' + err));


    this.warehouseHubConnection
      .start()
      .then(() => console.log('WarehouseHub connection started'))
      .catch(err => console.log('Error while starting WarehouseHub connection: ' + err));

    this.engineHubConnection.on('EngineReady', (order: any) => {
      this.orders$.subscribe(orders => {
        const foundOrder = orders.find(x => x.id === order.orderId);
        if (foundOrder) {
          foundOrder.isEngineProduced = true;
        }
      });
    });

    this.chassisHubConnection.on('ChassisReady', (order: any) => {
      this.orders$.subscribe(orders => {
        const foundOrder = orders.find(x => x.id === order.orderId);
        if (foundOrder) {
          foundOrder.isChassisProduced = true;
        }
      });
    });

    this.optionHubConnection.on('OptionReady', (order: any) => {
      this.orders$.subscribe(orders => {
        const foundOrder = orders.find(x => x.id === order.orderId);
        if (foundOrder) {
          foundOrder.isOptionPackProduced = true;
        }
      });
    });

    this.orderHubConnection.on('IsReadyForCollection', (orderId: any) => {
      this.orders$.subscribe(orders => {
        const foundOrder = orders.find(x => x.id === orderId);
        if (foundOrder) {
          foundOrder.isReadyForCollection = true;
        }
      });
    });

    this.warehouseHubConnection.on('EngineReady2', (order: any) => {
      debugger
      this.observable.next(order);
    });
  }

  private stopSignalRConnections(): void {
    this.engineHubConnection.stop().catch(err => console.error(err));
    this.chassisHubConnection.stop().catch(err => console.error(err));
    this.optionHubConnection.stop().catch(err => console.error(err));
    this.orderHubConnection.stop().catch(err => console.error(err));
    this.warehouseHubConnection.stop().catch(err => console.error(err));
  }

  ngOnDestroy() {
    this.stopSignalRConnections();
  }

  addProduct() {
    this.router.navigate(['add-product']);
  }

  async loadOrders() {
    this.hasLoaded = true;
    console.log("Starting loadOrders...");

    this.orders$.next([]);

    return lastValueFrom(this.orderService.getOrders())
      .then(async (res) => {
        if (res.length === 0) {
          this.message = 'No orders so far...';
        } else {
          this.orders$.next(res);
          await this.changeOrderState(res);
          return this.orders$;
        }
        return null;
      })
      .catch((err) => {
        console.error(err);
      });
  }

  async changeOrderState(orders) {
    for (let i = 0; i < orders.length; i++) {
      let order = orders[i] as OrderDTO;
      if (!order.isReadyForCollection) {
        const isEngineProduced = this.orderService.getEngineStatus(order.id.toString());
        let engineProduced = await lastValueFrom(isEngineProduced);
        if(!engineProduced){
          //check if its in stock instead or maybe it even already started in assembly
          const getProductStockStatus = this.orderService.getProductStockStatus(order.engineId.toString());
          let isInStock = await lastValueFrom(getProductStockStatus);
          order.isEngineProduced = isInStock;
        }
        else{
          order.isEngineProduced = engineProduced;
        }

        const isChassisProduced = this.orderService.getChassisStatus(order.id.toString());
        let chassisProduced = await lastValueFrom(isChassisProduced);
        if(!chassisProduced){
          //check if its in stock instead or maybe it even already started in assembly
          const getProductStockStatus = this.orderService.getProductStockStatus(order.chassisId.toString());
          let isInStock = await lastValueFrom(getProductStockStatus);
          order.isChassisProduced = isInStock;
        }
        else{
          order.isChassisProduced = chassisProduced;
        }
        const isOptionProduced = this.orderService.getOptionPackStatus(order.id.toString());
        let optionProduced = await lastValueFrom(isOptionProduced);
        if(!optionProduced){
          //check if its in stock instead or maybe it even already started in assembly
          const getProductStockStatus = this.orderService.getProductStockStatus(order.optionPackId.toString());
          let isInStock = await lastValueFrom(getProductStockStatus);
          order.isOptionPackProduced = isInStock;
        }
        else{
          order.isOptionPackProduced = optionProduced;
        }        
        console.log("order state finished");
      }
      else {
        order.isEngineProduced = true;
        order.isChassisProduced = true;
        order.isOptionPackProduced = true;
      }
    }
  }

  createOrder(): void {
    this.router.navigate(['/create-order']);
  }

  cancelOrder(orderId: string): void {
    this.orderService.cancelOrder(orderId).subscribe(response => {
      if (response) {
        this.loadOrders();
      } else {
        alert('Cannot cancel order');
      }
    });
  }
}