import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FarmDetailComponent } from './farm-detail.component';

describe('FarmDetailComponent', () => {
  let component: FarmDetailComponent;
  let fixture: ComponentFixture<FarmDetailComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [FarmDetailComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(FarmDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
