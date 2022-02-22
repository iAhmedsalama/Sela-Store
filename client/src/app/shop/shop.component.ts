import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { IBrand } from '../shared/models/brand';
import { IProduct } from '../shared/models/product';
import { IType } from '../shared/models/productTypes';
import { ShopService } from './shop.service';
import { ShopParams } from './../shared/models/shopParams';

@Component({
  selector: 'app-shop',
  templateUrl: './shop.component.html',
  styleUrls: ['./shop.component.scss'],
})
export class ShopComponent implements OnInit {

  //adding search properites
  @ViewChild('search', {static:true}) searchTerm?: ElementRef

  products?: IProduct[] = [];
  brands: IBrand[] = [];
  types: IType[] = [];

  //filtering properties
  brandIdSelected = 0;
  typeIdSelected = 0;

  //sorting properties
  sortSelected?:string = 'name';
  sortOptions = [
    { name: 'Alphabetical', value: 'name' },
    { name: 'Price: Low to High', value: 'priceAsc' },
    { name: 'Price: High to Low', value: 'priceDesc' },
  ];


  //adding shop params module
  shopParams = new ShopParams();

  //inject shop service
  constructor(private ShopService: ShopService) {}

  ngOnInit(): void {
    //consume methods
    this.getProducts();
    this.getBrands();
    this.getTypes();
  }

  getProducts() {
    this.ShopService.getProducts(this.brandIdSelected, this.typeIdSelected, this.sortSelected).subscribe(
      response => {
        this.products = response?.data;
      },error => {
        console.log(error);
      }
    );
  }

  //filtering methods
  getBrands() {
    this.ShopService.getBrands().subscribe(
      response => {
        this.brands = [{ id: 0, name: 'All' }, ...response];
      },
      error => {
        console.log(error);
      }
    );
  }

  getTypes() {
    this.ShopService.getTypes().subscribe(
      response => {
        this.types = [{ id: 0, name: 'All' }, ...response];
      },
      error => {
        console.log(error);
      }
    );
  }

  onBrandSelected(brandId: number) {
    this.brandIdSelected = brandId;
    this.getProducts();
  }

  onTypeSelected(typeId: number) {
    this.typeIdSelected = typeId;
    this.getProducts();
  }

  //sorting methods
  onSortSelected(sort: string) {
    this.sortSelected = sort;
    this.getProducts();
  }

  //pagination method
  onPageChanged(event: any){
  
  }

  //search method
  onSearch(){
    this.shopParams.search = this.searchTerm?.nativeElement.value;
    this.getProducts();
  }

  onRest(){
    this.searchTerm?.nativeElement.value;
    this.shopParams = new ShopParams();
    this.getProducts();
  }
}
