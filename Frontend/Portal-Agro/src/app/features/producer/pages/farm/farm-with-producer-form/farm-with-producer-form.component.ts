import { Component, inject } from '@angular/core';
import { FarmFormComponent } from "../farm-form/farm-form.component";
import { FarmSelectModel } from '../../../../../shared/models/farm/farm.model';
import Swal from 'sweetalert2';
import { take } from 'rxjs';
import { AuthState } from '../../../../../Core/services/auth/auth.state';

@Component({
  selector: 'app-farm-with-producer-form',
  imports: [FarmFormComponent],
  templateUrl: './farm-with-producer-form.component.html',
  styleUrl: './farm-with-producer-form.component.css'
})
export class FarmWithProducerFormComponent {
  // private auth = inject(AuthState);

  onSaved(farm: FarmSelectModel) {
    console.log('Finca guardada', farm);

    // this.auth.loadMe().pipe(take(1)).subscribe();
  }
}
