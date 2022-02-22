import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NavBarComponent } from './nav-bar/nav-bar.component';
import { RouterModule } from '@angular/router';



@NgModule({
  //import navbar
  declarations: [NavBarComponent],
  imports: [
    CommonModule,
    RouterModule
  ],
  exports:[
    //export navbar to use it in app module
    NavBarComponent
  ]
})
export class CoreModule { }
