import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DeparmentListComponent } from './deparment-list.component';

describe('DeparmentListComponent', () => {
  let component: DeparmentListComponent;
  let fixture: ComponentFixture<DeparmentListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DeparmentListComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DeparmentListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
