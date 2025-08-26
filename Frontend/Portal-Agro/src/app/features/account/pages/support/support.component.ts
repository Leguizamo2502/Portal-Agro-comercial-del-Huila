import { Component } from '@angular/core';
import { MatIconModule } from "@angular/material/icon";

@Component({
  selector: 'app-support',
  imports: [MatIconModule],
  templateUrl: './support.component.html',
  styleUrl: './support.component.css'
})
export class SupportComponent {
  email = 'portalagrocomercialhuila@gmail.com';
  phone = '+57 310 123 4567';
}
