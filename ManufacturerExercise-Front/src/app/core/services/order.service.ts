import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { ProductDTO } from '../models/Product.dto';

@Injectable({
  providedIn: 'root'
})
export class OrderService {
  private orderApiUrl = 'http://localhost:5229';
  private engineApiUrl = 'http://localhost:5372';
  private chassisApiUrl = 'http://localhost:5207'
  private optionPackApiUrl = 'http://localhost:5134'
  private warehouseApiUrl = 'http://localhost:5297'
  constructor(private httpRequest: HttpClient) { }

  // ORDERS
  getOrders(): Observable<any> {
    try {
      let url = `${this.orderApiUrl}/Order/GetOrders`;
      return this.httpRequest.get(url);
    } catch (error) {
      console.error(error);
      throw error;
    }
  }

  createOrder(order: any): Observable<any> {
    try {
      let url = `${this.orderApiUrl}/Order/CreateOrder`;
      return this.httpRequest.post(url, order);
    } catch (error) {
      console.error(error);
      throw error;
    }
  }

  cancelOrder(id: string): Observable<any> {
    try {
      let url = `${this.orderApiUrl}/Order/ChangeOrderStatus`;
      let request = { orderId : id, type: "IsCanceled", statusValue: true};
      return this.httpRequest.post(url, request);
    } catch (error) {
      console.error(error);
      throw error;
    }
  }

  getEngineStatus(orderId:string): Observable<any> {
    try {
      let url = `${this.engineApiUrl}/Engine/GetEngineProductionStatus?id=${orderId}`;
      return this.httpRequest.get(url);
    } catch (error) {
      console.error(error);
      throw error;
    }
  }

  getProductStockStatus(productId:string): Observable<boolean>{
    try {
      let url = `${this.warehouseApiUrl}/Warehouse/ReadOnlyStock?prodId=${productId}`;
      return this.httpRequest.get<boolean>(url);
    } catch (error) {
      console.error(error);
      throw error;
    }
  }

  getChassisStatus(orderId:string): Observable<any> {
    try {
      let url = `${this.chassisApiUrl}/Chassis/GetChassisProductionStatus?id=${orderId}`;
      return this.httpRequest.get(url);
    } catch (error) {
      console.error(error);
      throw error;
    }
  }

  getOptionPackStatus(orderId:string): Observable<any> {
    try {
      let url = `${this.optionPackApiUrl}/OptionPack/GetOptionPackProductionStatus?id=${orderId}`;
      return this.httpRequest.get(url);
    } catch (error) {
      console.error(error);
      throw error;
    }
  }

  // PRODUCTS
  createProduct(product: ProductDTO): Observable<any> {
    try {
      let url = `${this.warehouseApiUrl}/Warehouse/CreateProduct`;
      return this.httpRequest.post(url, product);
    } catch (error) {
      console.error(error);
      throw error;
    }
  }

  getProducts(): Observable<any> {
    try {
      let url = `${this.warehouseApiUrl}/Warehouse/GetProducts`;
      return this.httpRequest.get(url);
    } catch (error) {
      console.error(error);
      throw error;
    }
  }
}
