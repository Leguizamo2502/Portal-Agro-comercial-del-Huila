import { Component, Inject } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ProducerSelectModel, ProducerSocialCreateModel, SocialNetwork } from '../../../../shared/models/producer/producer.model';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialog, MatDialogModule } from '@angular/material/dialog';
import { MatInputModule } from "@angular/material/input";
import { MatSelectModule } from "@angular/material/select";
import { MatIconModule } from "@angular/material/icon";
import { CommonModule } from '@angular/common';
import { MatDividerModule } from "@angular/material/divider";
import { MatButtonModule } from '@angular/material/button';

// Validador: redes únicas
const distinctNetworksValidator = (fa: FormArray) => {
  const vals = fa.controls.map(c => c.get('network')?.value);
  const set = new Set(vals);
  return set.size !== vals.length ? { duplicateNetwork: true } : null;
};

export interface ProfileDialogData {
  producer?: ProducerSelectModel; 
}

@Component({
  selector: 'app-profile-dialog',
  imports: [MatInputModule, MatSelectModule, MatIconModule, ReactiveFormsModule, CommonModule, MatDividerModule,MatButtonModule,MatDialogModule],
  templateUrl: './profile-dialog.component.html',
  styleUrl: './profile-dialog.component.css'
})
export class ProfileDialogComponent {
  SocialNetwork = SocialNetwork;

  form: FormGroup;
  loading = false;

  get socialLinksFA(): FormArray {
    return this.form.get('socialLinks') as FormArray;
  }
  get socialLinksControls() {
    return this.socialLinksFA.controls as FormGroup[];
  }

  constructor(
    private fb: FormBuilder,
    public dialogRef: MatDialogRef<ProfileDialogComponent, { description: string; socialLinks?: ProducerSocialCreateModel[] }>,
    @Inject(MAT_DIALOG_DATA) public data: ProfileDialogData
  ) {
    this.form = this.fb.group({
      description: [data.producer?.description ?? '', [Validators.maxLength(500)]],
      socialLinks: this.fb.array([], { validators: (ctrl) => distinctNetworksValidator(ctrl as FormArray) })
    });

    // precargar redes si vienen
    const nets = data.producer?.networks ?? [];
    if (nets.length) {
      nets.forEach(n => this.socialLinksFA.push(this.fb.group({
        network: [n.network, [Validators.required]],
        url: [n.url, [Validators.required, Validators.maxLength(512)]]
      })));
    } else {
      // opcional: deja vacío; el usuario añade si quiere
      // this.addSocialLink();
    }
  }

  addSocialLink(): void {
    // sugiere la siguiente red no usada
    const used = new Set(this.socialLinksFA.controls.map(c => c.get('network')?.value));
    const order = [SocialNetwork.Whatsapp, SocialNetwork.Instagram, SocialNetwork.Facebook, SocialNetwork.X, SocialNetwork.Website];
    const next = order.find(x => !used.has(x)) ?? SocialNetwork.Website;

    this.socialLinksFA.push(this.fb.group({
      network: [next, [Validators.required]],
      url: ['', [Validators.required, Validators.maxLength(512)]]
    }));
  }

  removeSocialLink(i: number): void {
    this.socialLinksFA.removeAt(i);
  }

  submit(): void {
    if (this.loading) return;
    this.form.markAllAsTouched();
    if (this.form.invalid) return;

    const description = (this.form.value.description ?? '').trim();
    // filtra filas sin URL
    const items = (this.form.value.socialLinks as ProducerSocialCreateModel[] | undefined)
      ?.filter(x => x && x.url && String(x.url).trim().length > 0);

    // Si no hay filas válidas, envía undefined para no tocar redes (o [] si quieres limpiar)
    const payload = {
      description,
      socialLinks: items && items.length ? items : undefined
      // si quieres “limpiar todas”, usa socialLinks: []
    };

    this.dialogRef.close(payload);
  }
}
