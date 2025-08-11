import { Component } from '@angular/core';
import { NavbarVerticalComponent } from "../../../../shared/components/navbar-vertical/navbar-vertical.component";
import {  RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-account',
  imports: [NavbarVerticalComponent, RouterOutlet],
  templateUrl: './account.component.html',
  styleUrl: './account.component.css'
})
export class AccountComponent {

}
