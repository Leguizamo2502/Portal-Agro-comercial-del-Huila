import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RolFormPermissionFormComponent } from './rol-form-permission-form.component';

describe('RolFormPermissionFormComponent', () => {
  let component: RolFormPermissionFormComponent;
  let fixture: ComponentFixture<RolFormPermissionFormComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RolFormPermissionFormComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(RolFormPermissionFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
