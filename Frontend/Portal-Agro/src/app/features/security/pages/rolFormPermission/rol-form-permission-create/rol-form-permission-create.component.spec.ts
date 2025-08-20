import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RolFormPermissionCreateComponent } from './rol-form-permission-create.component';

describe('RolFormPermissionCreateComponent', () => {
  let component: RolFormPermissionCreateComponent;
  let fixture: ComponentFixture<RolFormPermissionCreateComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RolFormPermissionCreateComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(RolFormPermissionCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
