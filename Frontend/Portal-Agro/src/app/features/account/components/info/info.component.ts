import { Component, inject, OnInit } from '@angular/core';
import { PersonService } from '../../../../shared/services/person/person.service';
import { PersonSelectModel } from '../../../../shared/models/person/person.model';
import { CommonModule } from '@angular/common';
import { MatIconModule } from "@angular/material/icon";

@Component({
  selector: 'app-info',
  imports: [CommonModule, MatIconModule],
  templateUrl: './info.component.html',
  styleUrl: './info.component.css'
})
export class InfoComponent implements OnInit{
  personService = inject(PersonService);
  person? : PersonSelectModel;


  ngOnInit(): void {
    this.loadPerson();
  }

  loadPerson(){
    this.personService.getDataBasic().subscribe((data)=>{
      this.person = data;
    })
  }

}
