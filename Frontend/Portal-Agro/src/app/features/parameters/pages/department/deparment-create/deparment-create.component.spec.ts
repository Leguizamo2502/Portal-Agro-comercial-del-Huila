import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DepartmentCreateComponent } from './deparment-create.component';

describe('DeparmentCreateComponent', () => {
  let component: DepartmentCreateComponent;
  let fixture: ComponentFixture<DepartmentCreateComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DepartmentCreateComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DepartmentCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
