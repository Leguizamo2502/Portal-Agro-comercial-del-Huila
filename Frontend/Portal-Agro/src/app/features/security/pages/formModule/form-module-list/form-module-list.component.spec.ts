import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FormModuleListComponent } from './form-module-list.component';

describe('FormModuleListComponent', () => {
  let component: FormModuleListComponent;
  let fixture: ComponentFixture<FormModuleListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [FormModuleListComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(FormModuleListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
