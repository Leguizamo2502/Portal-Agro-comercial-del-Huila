import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

// Componentes standalone
import { CardComponent } from './components/card/card.component';
import { ButtonComponent } from './components/button/button.component';
import { NavbarComponent } from './components/navbar/navbar.component';

@NgModule({
  imports: [
    CommonModule,
    CardComponent,
    ButtonComponent,
    NavbarComponent
  ],
  exports: [
    CardComponent,
    ButtonComponent,
    NavbarComponent
  ]
})
export class SharedModule { }
