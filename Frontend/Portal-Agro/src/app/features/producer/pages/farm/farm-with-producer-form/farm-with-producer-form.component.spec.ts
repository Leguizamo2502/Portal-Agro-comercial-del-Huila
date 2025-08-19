import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FarmWithProducerFormComponent } from './farm-with-producer-form.component';

describe('FarmWithProducerFormComponent', () => {
  let component: FarmWithProducerFormComponent;
  let fixture: ComponentFixture<FarmWithProducerFormComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [FarmWithProducerFormComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(FarmWithProducerFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
