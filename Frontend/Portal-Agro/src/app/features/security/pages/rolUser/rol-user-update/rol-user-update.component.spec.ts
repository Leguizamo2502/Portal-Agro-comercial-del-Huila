import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RolUserUpdateComponent } from './rol-user-update.component';

describe('RolUserUpdateComponent', () => {
  let component: RolUserUpdateComponent;
  let fixture: ComponentFixture<RolUserUpdateComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RolUserUpdateComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(RolUserUpdateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
