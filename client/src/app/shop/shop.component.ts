import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { IBrand } from '../shared/models/brand';
import { IProduct } from '../shared/models/product';
import { IType } from '../shared/models/productTypes';
import { ShopService } from './shop.service';
import { ShopParams } from './../shared/models/shopParams';
import { IPagination } from '../shared/models/pagination';

@Component({
  selector: 'app-shop',
  templateUrl: './shop.component.html',
  styleUrls: ['./shop.component.scss'],
})
export class ShopComponent implements OnInit {
  //adding search properites
  @ViewChild('search', { static: true }) searchTerm?: ElementRef;

  products?: IProduct[] = [];
  brands: IBrand[] = [];
  types: IType[] = [];

  //filtering and sorting properties from shop params class
  shopParams = new ShopParams();

  //pagination count
  totalCount?: number;

  sortOptions = [
    { name: 'Alphabetical', value: 'name' },
    { name: 'Price: Low to High', value: 'priceAsc' },
    { name: 'Price: High to Low', value: 'priceDesc' },
  ];

  //inject shop service
  constructor(private ShopService: ShopService) {}

  ngOnInit(): void {
    //consume methods
    this.getProducts();
    this.getBrands();
    this.getTypes();
  }

  getProducts() {
    this.ShopService.getProducts(this.shopParams).subscribe(
      (response) => {
        this.products = response?.data;
        //adding pagination properties
        this.shopParams.pageNumber = response?.pageIndex;
        this.shopParams.pageSize = response?.pageSize;
        this.totalCount = response?.count;
      },
      (error) => {
        console.log(error);
      }
    );
  }

  //filtering methods
  getBrands() {
    this.ShopService.getBrands().subscribe(
      (response) => {
        //adding All to cancel filtering
        this.brands = [{ id: 0, name: 'All' }, ...response];
      },
      (error) => {
        console.log(error);
      }
    );
  }

  getTypes() {
    this.ShopService.getTypes().subscribe(
      (response) => {
        //adding All to cancel filtering
        this.types = [{ id: 0, name: 'All' }, ...response];
      },
      (error) => {
        console.log(error);
      }
    );
  }

  onBrandSelected(brandId: number) {
    this.shopParams.brandId = brandId;

    //reseting page number to page number 1 after using brand filter
    this.shopParams.pageNumber = 1;
    this.getProducts();
  }

  onTypeSelected(typeId: number) {
    this.shopParams.typeId = typeId;

    //reseting page number to page number 1 after using type filter
    this.shopParams.pageNumber = 1;
    this.getProducts();
  }

  //sorting methods
  onSortSelected(sort: string) {
    this.shopParams.sort = sort;
    this.getProducts();
  }

  //pagination method
  onPageChanged(event: any) {
    //the onPagechanged fires every time totalCount changed 
    //check if it fires or not
    if (this.shopParams.pageNumber !==event) {
      this.shopParams.pageNumber = event;
      this.getProducts();
    }
  }

  //search method
  onSearch() {
    this.shopParams.search = this.searchTerm?.nativeElement.value;

    //reseting page number to page number 1 after using search filter
    this.shopParams.pageNumber = 1;
    this.getProducts();
  }

  onRest() {
    this.searchTerm.nativeElement.value = '';
    this.shopParams = new ShopParams();
    this.getProducts();
  }
}
