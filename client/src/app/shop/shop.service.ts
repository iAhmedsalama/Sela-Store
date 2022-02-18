import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { IPagination } from '../shared/models/pagination';

//shop service to inject our service in it instead of inject service in app.component
@Injectable({
  providedIn: 'root',
})
export class ShopService {
  baseUrl = 'https://localhost:5001/api';

  constructor(private http: HttpClient) {}
  getProducts() {
    return this.http.get<IPagination>(this.baseUrl + 'products'); //when you have pagination
    // return this.http.get(this.baseUrl + 'products');
  }
}
