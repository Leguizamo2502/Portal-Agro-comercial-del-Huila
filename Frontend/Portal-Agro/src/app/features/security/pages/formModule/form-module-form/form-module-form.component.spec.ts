import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FormModuleFormComponent } from './form-module-form.component';

describe('FormModuleFormComponent', () => {
  let component: FormModuleFormComponent;
  let fixture: ComponentFixture<FormModuleFormComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [FormModuleFormComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(FormModuleFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
