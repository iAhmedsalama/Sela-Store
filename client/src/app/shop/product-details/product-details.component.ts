import { Component, OnInit } from '@angular/core';
import { IProduct } from './../../shared/models/product';
import { ShopService } from './../shop.service';
import { ActivatedRoute } from '@angular/router';
import { BreadcrumbService } from 'xng-breadcrumb';
import { BasketService } from 'src/app/basket/basket.service';

@Component({
  selector: 'app-product-details',
  templateUrl: './product-details.component.html',
  styleUrls: ['./product-details.component.scss'],
})
export class ProductDetailsComponent implements OnInit {

  product: IProduct;

  //adding basket functionality
  quantity = 1;

  //inject shopService service which have http client function
  //inject activatedRoute service which used to get parameters from activatedRoute
  //inject breadcrumb service
  constructor(private shopService: ShopService,
              private activatedRoute: ActivatedRoute, 
              private breadcrumb: BreadcrumbService,
              private basketService: BasketService) {}

  ngOnInit(): void {
    this.loadProduct();
  }

  loadProduct() {
    //+ casting operator to convert to id to number
    this.shopService.getProduct(+this.activatedRoute.snapshot.paramMap.get('id')).subscribe(
      (product) => {
        this.product = product;
        //using breadcrumb service to get the product name from the alias name
        this.breadcrumb.set('@productDetails', product.name); 
      },
      (error) => {
        console.log(error);
      }
    );
  }

  //adding basket functionality
  addItemToBasket(){
    this.basketService.addItemToBasket(this.product, this.quantity);
  }

  incrementQuantity(){
    this.quantity++;
  }

  decrementQuantity(){
    if(this.quantity > 1)
      this.quantity--;
  }
}
