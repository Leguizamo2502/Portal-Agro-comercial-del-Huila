import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FarmingBannerComponent } from './farming-banner.component';

describe('FarmingBannerComponent', () => {
  let component: FarmingBannerComponent;
  let fixture: ComponentFixture<FarmingBannerComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [FarmingBannerComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(FarmingBannerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
