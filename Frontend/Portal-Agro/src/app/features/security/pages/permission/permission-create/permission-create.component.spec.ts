import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PermissionCreateComponent } from './permission-create.component';

describe('PermissionCreateComponent', () => {
  let component: PermissionCreateComponent;
  let fixture: ComponentFixture<PermissionCreateComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PermissionCreateComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PermissionCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
