import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RolUserCreateComponent } from './rol-user-create.component';

describe('RolUserCreateComponent', () => {
  let component: RolUserCreateComponent;
  let fixture: ComponentFixture<RolUserCreateComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RolUserCreateComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(RolUserCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
