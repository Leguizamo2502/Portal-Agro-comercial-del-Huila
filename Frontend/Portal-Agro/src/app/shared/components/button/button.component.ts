import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'app-button',
  imports: [CommonModule],
  templateUrl: './button.component.html',
  styleUrl: './button.component.css'
})
export class ButtonComponent {
  @Input() text: string = 'Botón';
  @Input() type: 'button' | 'submit' | 'reset' = 'button';
  @Input() disabled: boolean = false;
  @Input() color: 'primary' | 'secondary' | 'danger' = 'primary';

  @Output() clicked = new EventEmitter<void>();

  onClick() {
    if (!this.disabled) {
      this.clicked.emit();
    }
  }
}
