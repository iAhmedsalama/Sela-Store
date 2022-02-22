import { Component, OnInit } from '@angular/core';
import { IProduct } from './../../shared/models/product';
import { ShopService } from './../shop.service';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-product-details',
  templateUrl: './product-details.component.html',
  styleUrls: ['./product-details.component.scss'],
})
export class ProductDetailsComponent implements OnInit {

  product: IProduct;

  //inject shopService service which have http client function
  //inject activatedRoute service which used to get parameters from activatedRoute
  constructor(private shopService: ShopService, private activatedRoute: ActivatedRoute) {}

  ngOnInit(): void {
    this.loadProduct();
  }

  loadProduct() {
    //+ casting operator to convert to id to number
    this.shopService.getProduct(+this.activatedRoute.snapshot.paramMap.get('id')).subscribe(
      (product) => {
        this.product = product;
      },
      (error) => {
        console.log(error);
      }
    );
  }
}
