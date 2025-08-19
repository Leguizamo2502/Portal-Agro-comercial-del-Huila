import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CardFarmComponent } from './card-farm.component';

describe('CardFarmComponent', () => {
  let component: CardFarmComponent;
  let fixture: ComponentFixture<CardFarmComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CardFarmComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CardFarmComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
