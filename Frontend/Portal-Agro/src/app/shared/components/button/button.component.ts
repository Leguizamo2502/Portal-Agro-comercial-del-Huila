import { CommonModule, Location } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { MatIconModule } from '@angular/material/icon';
import { Router } from '@angular/router';

@Component({
  selector: 'app-button',
  standalone: true,
  imports: [CommonModule, MatIconModule],
  templateUrl: './button.component.html',
  styleUrls: ['./button.component.css'],
})
export class ButtonComponent {
  @Input() text: string = 'Botón';
  @Input() type: 'button' | 'submit' | 'reset' = 'button';
  @Input() disabled: boolean = false;
  @Input() color: 'primary' | 'secondary' | 'danger' = 'primary';
  @Input() icon: string = '';

  /** Si es true, ejecuta Location.back() */
  @Input() back: boolean = false;

  /** Si se establece, redirige a esta ruta al hacer clic */
  @Input() redirectTo: string | null = null;

  @Output() clicked = new EventEmitter<void>();

  constructor(private location: Location, private router: Router) {}

  onClick() {
    if (this.disabled) return;

    if (this.back) {
      this.location.back();
    } else if (this.redirectTo) {
      this.router.navigate([this.redirectTo]);
    }

    this.clicked.emit();
  }
}
