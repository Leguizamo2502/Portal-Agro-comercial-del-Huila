import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FormChangePasswordComponent } from './form-change-password.component';

describe('FormChangePasswordComponent', () => {
  let component: FormChangePasswordComponent;
  let fixture: ComponentFixture<FormChangePasswordComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [FormChangePasswordComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(FormChangePasswordComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
