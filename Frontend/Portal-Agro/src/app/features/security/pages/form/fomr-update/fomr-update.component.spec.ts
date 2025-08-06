import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FomrUpdateComponent } from './fomr-update.component';

describe('FomrUpdateComponent', () => {
  let component: FomrUpdateComponent;
  let fixture: ComponentFixture<FomrUpdateComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [FomrUpdateComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(FomrUpdateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
