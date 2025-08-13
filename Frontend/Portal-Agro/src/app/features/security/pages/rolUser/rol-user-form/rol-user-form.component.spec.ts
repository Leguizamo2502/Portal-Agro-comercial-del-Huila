import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RolUserFormComponent } from './rol-user-form.component';

describe('RolUserFormComponent', () => {
  let component: RolUserFormComponent;
  let fixture: ComponentFixture<RolUserFormComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RolUserFormComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(RolUserFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
