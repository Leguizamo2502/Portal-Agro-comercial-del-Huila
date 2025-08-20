import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ContainerCardFlexComponent } from './container-card-flex.component';

describe('ContainerCardFlexComponent', () => {
  let component: ContainerCardFlexComponent;
  let fixture: ComponentFixture<ContainerCardFlexComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ContainerCardFlexComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ContainerCardFlexComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
