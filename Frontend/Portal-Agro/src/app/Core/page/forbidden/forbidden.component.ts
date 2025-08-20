import { Component, inject, OnInit } from '@angular/core';
import { AuthState } from '../../services/auth/auth.state';

@Component({
  selector: 'app-forbidden',
  imports: [],
  templateUrl: './forbidden.component.html',
  styleUrl: './forbidden.component.css'
})
export class ForbiddenComponent implements OnInit{
  private auth = inject(AuthState);
  
  ngOnInit(): void {
    this.auth.loadMe().subscribe(()=>{
      console.log("No autorizado")
    })
  }
  

}
