import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProducerOrdersListComponent } from './producer-orders-list.component';

describe('ProducerOrdersListComponent', () => {
  let component: ProducerOrdersListComponent;
  let fixture: ComponentFixture<ProducerOrdersListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ProducerOrdersListComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ProducerOrdersListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
