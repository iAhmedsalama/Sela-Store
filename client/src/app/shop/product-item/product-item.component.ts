import { Component, Input, OnInit } from '@angular/core';
import { BasketService } from 'src/app/basket/basket.service';
import { IProduct } from 'src/app/shared/models/product';

@Component({
  selector: 'app-product-item',
  templateUrl: './product-item.component.html',
  styleUrls: ['./product-item.component.scss'],
})
export class ProductItemComponent implements OnInit {
  //import data from parent component
  @Input()
  product: IProduct = {
    id: 0,
    name: '',
    description: '',
    price: 0,
    pictureUrl: '',
    productType: '',
    productBrand: '',
  };

  //inject basket service to use it
  constructor(private basketService: BasketService) {}

  ngOnInit(): void {}

  addItemToBasket() {
    this.basketService.addItemToBasket(this.product);
  }
}
