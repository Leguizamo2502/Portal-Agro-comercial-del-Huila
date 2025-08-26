import { Injectable } from '@angular/core';
import { ValidatorFn, AbstractControl, ValidationErrors, FormGroup } from '@angular/forms';

@Injectable({
  providedIn: 'root',
})
export class PasswordPolicyService {
  // Misma regla que en el backend: ^(?=.*[A-Z]).{6,}$
  private readonly regex = /^(?=.*[A-Z]).{6,}$/;

  /** Chequeo directo (útil fuera de forms) */
  isValid(password: string | null | undefined): boolean {
    if (!password) return false;
    return this.regex.test(password);
  }

  /** Validator para un FormControl */
  validator(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      const value = control.value as string;
      if (!value) return { required: true };
      return this.regex.test(value) ? null : { passwordPolicy: true };
    };
  }

  /**
   * Validator de coincidencia entre dos campos del mismo FormGroup.
   * Ej.: passwordsMatch('newPassword', 'confirmNewPassword')
   */
  passwordsMatch(field: string, confirmField: string): ValidatorFn {
    return (group: AbstractControl): ValidationErrors | null => {
      const fg = group as FormGroup;
      const a = fg.get(field)?.value;
      const b = fg.get(confirmField)?.value;
      if (!a || !b) return null;
      return a === b ? null : { passwordMismatch: true };
    };
  }
}
