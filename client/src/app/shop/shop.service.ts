import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { IPagination } from '../shared/models/pagination';
import { IBrand } from '../shared/models/brand';
import { IType } from '../shared/models/productTypes';
import { map } from 'rxjs/operators';
import { ShopParams } from './../shared/models/shopParams';
import { IProduct } from './../shared/models/product';

//shop service to inject our service in [shop.component] it instead of inject service in [app.component]
@Injectable({
  providedIn: 'root',
})
export class ShopService {
  baseUrl = 'https://localhost:5001/api/';

  constructor(private http: HttpClient) {}

  getProducts(shopParams: ShopParams) {
    let params = new HttpParams();

    //adding filtering by brand id
    if (shopParams.brandId !== 0) {
      params = params.append('brandId', shopParams.brandId.toString());
    }

    //adding filtering by type id
    if (shopParams.typeId !== 0) {
      params = params.append('typeId', shopParams.typeId.toString());
    }

    //adding sorting functionality
    params = params.append('sort', shopParams.sort);

    //adding pagination functionality
    params = params.append('pageIndex', shopParams.pageNumber.toString());
    params = params.append('pageIndex', shopParams.pageSize.toString());

    //adding search functionality
    if (shopParams.search) {
      params = params.append('search', shopParams.search);
    }
    return this.http
      .get<IPagination>(this.baseUrl + 'products', {
        observe: 'response',
        params,
      })
      .pipe(
        map((response) => {
          return response.body;
        })
      );
  }

  //get brands method
  getBrands() {
    return this.http.get<IBrand[]>(this.baseUrl + 'products/brands');
  }

  //get types method
  getTypes() {
    return this.http.get<IType[]>(this.baseUrl + 'products/types');
  }

  //get product by id
  getProduct(id: number){
    return this.http.get<IProduct>(this.baseUrl + 'products/' + id);
  }
}
