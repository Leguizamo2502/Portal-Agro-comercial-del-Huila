import { Component } from '@angular/core';
import { NavbarComponent } from "../../../../shared/components/navbar/navbar.component";
import { NavbarVerticalComponent } from "../../../../shared/components/navbar-vertical/navbar-vertical.component";
import {  RouterOutlet } from '@angular/router';
import { NavbarBuenoComponent } from "../../../../shared/components/navbar-bueno/navbar-bueno.component";

@Component({
  selector: 'app-account',
  imports: [NavbarVerticalComponent, RouterOutlet, NavbarBuenoComponent],
  templateUrl: './account.component.html',
  styleUrl: './account.component.css'
})
export class AccountComponent {

}
