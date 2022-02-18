import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NavBarComponent } from './nav-bar/nav-bar.component';



@NgModule({
  //import navbar
  declarations: [NavBarComponent],
  imports: [
    CommonModule
  ],
  exports:[
    //export navbar to use it in app module
    NavBarComponent
  ]
})
export class CoreModule { }
