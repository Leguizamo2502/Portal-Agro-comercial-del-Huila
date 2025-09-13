import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ContainerCardProductorComponent } from './container-card-productor.component';

describe('ContainerCardProductorComponent', () => {
  let component: ContainerCardProductorComponent;
  let fixture: ComponentFixture<ContainerCardProductorComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ContainerCardProductorComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ContainerCardProductorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
