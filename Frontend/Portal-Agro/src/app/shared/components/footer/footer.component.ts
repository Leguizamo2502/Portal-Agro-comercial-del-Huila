import { Component } from '@angular/core';

@Component({
  selector: 'app-footer',
  templateUrl: './footer.component.html',
  styleUrls: ['./footer.component.css']
})
export class FooterComponent {

  email: string = "portalagrocomercialhuila@gmail.com";
  location: string = "Neiva, Huila  Colombia";

  scrollTo(sectionId: string, event: Event) {
    event.preventDefault(); 

    const element = document.getElementById(sectionId);
    if (element) {
      element.scrollIntoView({ behavior: 'smooth' });
    } else {
      console.warn('No se encontró el elemento con id:', sectionId);
    }
  }
}
