import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { OrderService } from '../../services/order.service';
import { ProductDTO } from '../../models/Product.dto';
import { OrderDTO } from '../../models/Order.dto';

@Component({
  selector: 'app-create-order',
  templateUrl: './create-order.component.html',
  styleUrls: ['./create-order.component.scss']
})
export class CreateOrderComponent {
  engines:Array<ProductDTO>;
  chasses:Array<ProductDTO>;
  optionPacks:Array<ProductDTO>;
  
  selectedEngine: string;
  selectedChassis: string;
  selectedOptionPack: string;

  order:OrderDTO = new OrderDTO();

  constructor(private orderService: OrderService, private router: Router) { 
    this.order = new OrderDTO();
    this.getProducts();
  }

  getProducts(): void{
    this.orderService.getProducts().subscribe(response => {
      if(response.length > 0){
        this.engines = response.filter(x => x.type == 0);
        this.chasses = response.filter(x => x.type == 1);
        this.optionPacks = response.filter(x => x.type == 2);
      }
    });
  }

  createOrder(): void {
    this.order = {
      //random customer id
      customerId: "c6e0a2ea-edf0-43de-abae-aa5c9541008f",
      orderDate: new Date().toJSON(),
      isReadyForCollection: false,
      isCanceled:false,
      engineId : this.selectedEngine,
      chassisId: this.selectedChassis,
      optionPackId: this.selectedOptionPack
    };

    this.orderService.createOrder(this.order).subscribe(response => {
      if (response) {
        this.router.navigate(['/dashboard']);
      } else {
        alert('Error creating order: ' + response.message);
      }
    });
  }
}
