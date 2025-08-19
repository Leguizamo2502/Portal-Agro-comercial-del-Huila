import { Component } from '@angular/core';
import { FarmFormComponent } from "../farm-form/farm-form.component";
import { FarmSelectModel } from '../../../../../shared/models/farm/farm.model';

@Component({
  selector: 'app-farm-create',
  imports: [FarmFormComponent],
  templateUrl: './farm-create.component.html',
  styleUrl: './farm-create.component.css'
})
export class FarmCreateComponent {
  onSaved(farm: FarmSelectModel) {
      console.log('Finca guardada', farm);
  

    }
}
