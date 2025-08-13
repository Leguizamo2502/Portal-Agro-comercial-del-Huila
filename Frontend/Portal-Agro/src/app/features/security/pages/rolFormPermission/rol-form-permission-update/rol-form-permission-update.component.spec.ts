import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RolFormPermissionUpdateComponent } from './rol-form-permission-update.component';

describe('RolFormPermissionUpdateComponent', () => {
  let component: RolFormPermissionUpdateComponent;
  let fixture: ComponentFixture<RolFormPermissionUpdateComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RolFormPermissionUpdateComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(RolFormPermissionUpdateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
