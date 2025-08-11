import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FarmFormComponent } from './farm-form.component';

describe('FarmFormComponent', () => {
  let component: FarmFormComponent;
  let fixture: ComponentFixture<FarmFormComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [FarmFormComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(FarmFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
