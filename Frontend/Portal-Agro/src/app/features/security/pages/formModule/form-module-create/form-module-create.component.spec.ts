import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FormModuleCreateComponent } from './form-module-create.component';

describe('FormModuleCreateComponent', () => {
  let component: FormModuleCreateComponent;
  let fixture: ComponentFixture<FormModuleCreateComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [FormModuleCreateComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(FormModuleCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
