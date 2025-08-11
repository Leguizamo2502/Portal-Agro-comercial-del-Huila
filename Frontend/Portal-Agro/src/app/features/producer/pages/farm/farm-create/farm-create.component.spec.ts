import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FarmCreateComponent } from './farm-create.component';

describe('FarmCreateComponent', () => {
  let component: FarmCreateComponent;
  let fixture: ComponentFixture<FarmCreateComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [FarmCreateComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(FarmCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
