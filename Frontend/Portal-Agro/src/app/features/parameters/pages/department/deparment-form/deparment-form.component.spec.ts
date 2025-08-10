import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DepartmentFormComponent } from './deparment-form.component';

describe('DeparmentFormComponent', () => {
  let component: DepartmentFormComponent;
  let fixture: ComponentFixture<DepartmentFormComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DepartmentFormComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DepartmentFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
