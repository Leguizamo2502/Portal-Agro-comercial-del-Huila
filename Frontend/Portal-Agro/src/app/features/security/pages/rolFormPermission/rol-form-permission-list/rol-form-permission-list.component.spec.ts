import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RolFormPermissionListComponent } from './rol-form-permission-list.component';

describe('RolFormPermissionListComponent', () => {
  let component: RolFormPermissionListComponent;
  let fixture: ComponentFixture<RolFormPermissionListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RolFormPermissionListComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(RolFormPermissionListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
