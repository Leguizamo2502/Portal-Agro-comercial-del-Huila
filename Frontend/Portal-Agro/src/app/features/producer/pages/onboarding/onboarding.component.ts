import { ChangeDetectionStrategy, Component, EventEmitter, Input, Output } from '@angular/core';
import { MatCardModule } from "@angular/material/card";
import { MatIconModule } from "@angular/material/icon";
import { ButtonComponent } from "../../../../shared/components/button/button.component";

@Component({
  selector: 'app-onboarding',
  imports: [MatCardModule, MatIconModule, ButtonComponent],
  templateUrl: './onboarding.component.html',
  styleUrl: './onboarding.component.css',
})

export class OnboardingComponent {
  // @Input() title = 'Conviértete en Productor';
  // @Input() subtitle = 'Crea tu perfil, registra tu finca y comienza a publicar tus productos.';
  // @Input() disabled = false;

 
}
