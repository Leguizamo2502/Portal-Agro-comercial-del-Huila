import { Component, inject, OnInit } from '@angular/core';
import { Router, RouterOutlet } from '@angular/router';
import { CommonModule } from '@angular/common';
import { AuthState } from './Core/services/auth/auth.state';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, CommonModule],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit{
  title = 'Portal-Agro';
  router = inject(Router);
  private authState = inject(AuthState);
   ngOnInit() {
    this.authState.hydrateFromStorage();
    
  }
}
