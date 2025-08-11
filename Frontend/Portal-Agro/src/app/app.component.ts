import { Component, inject } from '@angular/core';
import { Router, RouterOutlet } from '@angular/router';
import { CommonModule } from '@angular/common';
import { NavbarBuenoComponent } from "./shared/components/navbar-bueno/navbar-bueno.component";

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, CommonModule, NavbarBuenoComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = 'Portal-Agro';
  router = inject(Router);
}
