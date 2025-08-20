import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FarmUpdateComponent } from './farm-update.component';

describe('FarmUpdateComponent', () => {
  let component: FarmUpdateComponent;
  let fixture: ComponentFixture<FarmUpdateComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [FarmUpdateComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(FarmUpdateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
