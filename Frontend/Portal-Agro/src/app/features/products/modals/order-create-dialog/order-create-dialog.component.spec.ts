import { ComponentFixture, TestBed } from '@angular/core/testing';

import { OrderCreateDialogComponent } from './order-create-dialog.component';

describe('OrderCreateDialogComponent', () => {
  let component: OrderCreateDialogComponent;
  let fixture: ComponentFixture<OrderCreateDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [OrderCreateDialogComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(OrderCreateDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
