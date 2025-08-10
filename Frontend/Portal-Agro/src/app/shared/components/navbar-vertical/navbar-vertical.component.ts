import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatDividerModule } from '@angular/material/divider';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService } from '../../../Core/services/auth/auth.service';
import { UserSelectModel } from '../../../Core/Models/user.model';


@Component({
  selector: 'app-navbar-vertical',
  standalone: true,
  imports: [CommonModule, MatButtonModule, MatIconModule, MatDividerModule],
  templateUrl: './navbar-vertical.component.html',
  styleUrls: ['./navbar-vertical.component.css'],
})
export class NavbarVerticalComponent  implements OnInit{
  
  authService = inject(AuthService);
  router = inject(Router);
  route = inject(ActivatedRoute);
  activePath = '';
  user?:UserSelectModel
  
  ngOnInit(): void {
    this.loadUser();
  }

  loadUser(){
    this.authService.GetDataBasic().subscribe((data)=>{
      this.user= data;
    })
  }
  

  navigateTo(path: string) {
    this.router.navigate([path], { relativeTo: this.route });
    this.activePath = path;
    console.log("Hola")
  }

  
}

