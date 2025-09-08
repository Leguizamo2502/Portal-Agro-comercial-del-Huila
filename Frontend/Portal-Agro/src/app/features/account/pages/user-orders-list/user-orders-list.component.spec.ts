import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UserOrdersListComponent } from './user-orders-list.component';

describe('UserOrdersListComponent', () => {
  let component: UserOrdersListComponent;
  let fixture: ComponentFixture<UserOrdersListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [UserOrdersListComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(UserOrdersListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
