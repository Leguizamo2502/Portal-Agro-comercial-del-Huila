import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { NavbarVerticalComponent } from "../../../../shared/components/navs/navbar-vertical/navbar-vertical.component";
import {  RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-account',
  standalone: true,
  imports: [RouterOutlet],
  templateUrl: './account.component.html',
  styleUrls: ['./account.component.css']
})
export class AccountComponent {}
