import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatMenuModule } from '@angular/material/menu';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

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