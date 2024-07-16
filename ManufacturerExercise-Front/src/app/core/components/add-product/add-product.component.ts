import { Component, OnInit } from '@angular/core';
import { OrderService } from '../../services/order.service';
import { ProductDTO } from '../../models/Product.dto';

@Component({
  selector: 'app-add-product',
  templateUrl: './add-product.component.html',
  styleUrls: ['./add-product.component.scss']
})
export class AddProductComponent {
  product: ProductDTO = new ProductDTO();
  message:string;

  constructor(private orderService: OrderService) {
    this.product.name = '';
    this.product.type = 0;
    this.product.price = 0;
  }

  createProduct() {
    this.orderService.createProduct(this.product).subscribe(response => {
        if(response){
          this.message = 'Product created successfully!';
        }
        else{
          this.message = 'Product failed to create!';
        }
    });
  }
}
