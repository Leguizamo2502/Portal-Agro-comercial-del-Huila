import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatIconModule } from "@angular/material/icon";
import { AuthService } from '../../../../Core/services/auth/auth.service';
import { UserSelectModel } from '../../../../Core/Models/user.model';
import { ButtonComponent } from "../../../../shared/components/button/button.component";

@Component({
  selector: 'app-info',
  imports: [CommonModule, MatIconModule, ButtonComponent],
  templateUrl: './info.component.html',
  styleUrl: './info.component.css'
})
export class InfoComponent implements OnInit{
  authService = inject(AuthService);
  person? : UserSelectModel;


  ngOnInit(): void {
    this.loadPerson();
  }

  loadPerson(){
    this.authService.GetDataBasic().subscribe((data)=>{
      this.person = data;
      console.log(data);
    })
  }

}