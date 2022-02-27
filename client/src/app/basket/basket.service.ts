import { Injectable } from '@angular/core';
import { environment } from './../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject } from 'rxjs';
import { map } from 'rxjs/operators';
import { Basket, IBasket, IBasketTotals } from '../shared/models/basket';
import { IProduct } from './../shared/models/product';
import { IBasketItem } from './../shared/models/basket';

@Injectable({
  providedIn: 'root',
})
export class BasketService {
  baseUrl = environment.apiUrl;

  //adding BehaviorSubject of type IBasket
  private basketSource = new BehaviorSubject<IBasket>(null);

  //create a basket Observable
  basket$ = this.basketSource.asObservable();

  //adding basket summary
  //adding BehaviorSubject of type IBasketTotals
  private basketToatlSource = new BehaviorSubject<IBasketTotals>(null);

  //create a basketTotal Observable
  basketTotal$ = this.basketToatlSource.asObservable();


  constructor(private http: HttpClient) {}

  getBasket(id: string) {
    return this.http.get(this.baseUrl + 'basket?id=' + id).pipe(
      map((basket: any): void => {
        this.basketSource.next(basket);
        //adding summary totals
        this.calculateTotals();
        
      })
    );
  }

  setBasket(basket: IBasket) {
    return this.http
      .post(this.baseUrl + 'basket', basket)
      .subscribe((response: any): void => {
        this.basketSource.next(response);
       //adding summary totals
       this.calculateTotals();
        
      }, error =>{
        console.log(error);
        
      });
  }

  getCurrentBasketValue(){
    return this.basketSource.value;
  }

  addItemToBasket(item: IProduct, quantity= 1){

    const itemToAdd: IBasketItem = this.mapProductItemToBasketItem(item, quantity);
    const basket = this.getCurrentBasketValue() ?? this.createBasket();

    // console.log(basket);
    
    basket.items = this.addOrUpdateItem(basket.items, itemToAdd, quantity);
    this.setBasket(basket);
  }

  private addOrUpdateItem(items: IBasketItem[], itemToAdd: IBasketItem, quantity: number): IBasketItem[] {
    // console.log(items);
    
    const index = items.findIndex(i => i.id === itemToAdd.id);
    if (index === -1) {
      itemToAdd.quantity = quantity;
      items.push(itemToAdd);
    }else{
      items[index].quantity += quantity;
    }

    return items;
  }

  private createBasket(): IBasket{
    const basket = new Basket();
    localStorage.setItem('basket_id', basket.id);
    return basket;
  }

  private mapProductItemToBasketItem(item: IProduct, quantity: number): IBasketItem {
    return {
      id: item.id,
      productName: item.name,
      price: item.price,
      pictureUrl: item.pictureUrl,
      quantity,
      brand: item.productBrand,
      type: item.productType
    }
  }
  
  //adding total summaery
  private calculateTotals(){
    const basket = this.getCurrentBasketValue();
    const shipping = 0;
    const subtotal = basket.items.reduce((a, b) => (b.price * b.quantity) + a, 0);
    const total = subtotal + shipping;
    this.basketToatlSource.next({shipping, total, subtotal});
  }

  //adding increament item quantity
  increamentItemQuantity(item: IBasketItem){
    const basket = this.getCurrentBasketValue();
    const foundItemIndex = basket.items.findIndex(x => x.id === item.id);
    basket.items[foundItemIndex].quantity++;
    this.setBasket(basket);
  }

  //adding decreament item quantity
  decreamentItemQuantity(item: IBasketItem){
    const basket = this.getCurrentBasketValue();
    const foundItemIndex = basket.items.findIndex(x => x.id === item.id);
    if(basket.items[foundItemIndex].quantity > 1){

      basket.items[foundItemIndex].quantity--;
      this.setBasket(basket);

    }else{
      this.removeItemFromBasket(item);
    }
  }
  removeItemFromBasket(item: IBasketItem) {
    const basket = this.getCurrentBasketValue();
    if (basket.items.some(x=> x.id === item.id)) {
      basket.items = basket.items.filter(i=> i.id !== item.id);

      if (basket.items.length > 0) {
        this.setBasket(basket);
      }else{
        this.deleteBasket(basket);
      }

    }

  }
  deleteBasket(basket: IBasket) {
    return this.http.delete(this.baseUrl + 'basket?id=' + basket.id).subscribe(()=>{
      this.basketSource.next(null);
      this.basketToatlSource.next(null);
      localStorage.removeItem('basket_id');
    }, error => {
      console.log(error);
      
    })
  }
  
}
