import { CommonModule, Location } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { MatIconModule } from '@angular/material/icon';
import { MatStepperModule } from '@angular/material/stepper';
import { Router } from '@angular/router';

@Component({
  selector: 'app-button',
  standalone: true,
  imports: [CommonModule, MatIconModule,MatStepperModule],
  templateUrl: './button.component.html',
  styleUrls: ['./button.component.css'],
})
export class ButtonComponent {
  @Input() text: string = 'Botón';
  @Input() type: 'button' | 'submit' | 'reset' = 'button';
  @Input() disabled: boolean = false;
  @Input() color: 'primary' | 'secondary' | 'danger' = 'primary';
  @Input() icon: string = '';
  @Input() back: boolean = false;
  @Input() redirectTo: string | null = null;

  /** Acción del stepper */
  @Input() stepperAction: 'next' | 'previous' | null = null;

  @Output() clicked = new EventEmitter<void>();

  constructor(private location: Location, private router: Router) {}

  onClick() {
    if (this.disabled) return;
    if (this.back) this.location.back();
    else if (this.redirectTo) this.router.navigate([this.redirectTo]);
    this.clicked.emit();
  }
}
