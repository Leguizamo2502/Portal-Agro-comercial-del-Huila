import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DepartmentUpdateComponent } from './department-updte.component';

describe('DepartmentUpdteComponent', () => {
  let component: DepartmentUpdateComponent;
  let fixture: ComponentFixture<DepartmentUpdateComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DepartmentUpdateComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DepartmentUpdateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
