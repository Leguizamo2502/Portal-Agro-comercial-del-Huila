import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProducerLayoutComponent } from './producer-layout.component';

describe('ProducerLayoutComponent', () => {
  let component: ProducerLayoutComponent;
  let fixture: ComponentFixture<ProducerLayoutComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ProducerLayoutComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ProducerLayoutComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
