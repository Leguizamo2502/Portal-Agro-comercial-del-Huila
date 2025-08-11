import { Component } from '@angular/core';
import { HeroComponent } from '../../../../shared/components/hero/hero.component';
import { CarruselComponent } from '../../../../shared/components/carrusel/carrusel.component';
@Component({
  selector: 'app-home',
  imports: [HeroComponent,CarruselComponent],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent {

}
