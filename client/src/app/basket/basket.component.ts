import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { IBasket } from '../shared/models/basket';
import { BasketService } from './basket.service';
import { IBasketItem } from './../shared/models/basket';

@Component({
  selector: 'app-basket',
  templateUrl: './basket.component.html',
  styleUrls: ['./basket.component.scss']
})
export class BasketComponent implements OnInit {

  basket$: Observable<IBasket>;

  constructor(private basketService: BasketService) { }

  ngOnInit(): void {
    this.basket$ = this.basketService.basket$;
  }

  removeBasketItem(item: IBasketItem){
    this.basketService.removeItemFromBasket(item);
  }

  incrementItemQuantity(item: IBasketItem){
    this.basketService.increamentItemQuantity(item);
  }

  decrementItemQuantity(item: IBasketItem){
    this.basketService.decreamentItemQuantity(item);
  }
}
