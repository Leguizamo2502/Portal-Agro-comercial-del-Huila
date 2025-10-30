import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DriverTourComponent } from './driver-tour.component';

describe('DriverTourComponent', () => {
  let component: DriverTourComponent;
  let fixture: ComponentFixture<DriverTourComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DriverTourComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DriverTourComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
