import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProducerOrderDetailComponent } from './producer-order-detail.component';

describe('ProducerOrderDetailComponent', () => {
  let component: ProducerOrderDetailComponent;
  let fixture: ComponentFixture<ProducerOrderDetailComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ProducerOrderDetailComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ProducerOrderDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
