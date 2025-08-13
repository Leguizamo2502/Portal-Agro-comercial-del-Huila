import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FormModuleUpdateComponent } from './form-module-update.component';

describe('FormModuleUpdateComponent', () => {
  let component: FormModuleUpdateComponent;
  let fixture: ComponentFixture<FormModuleUpdateComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [FormModuleUpdateComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(FormModuleUpdateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
