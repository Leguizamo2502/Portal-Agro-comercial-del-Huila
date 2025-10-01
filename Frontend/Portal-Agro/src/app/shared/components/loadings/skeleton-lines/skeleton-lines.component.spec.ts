import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SkeletonLinesComponent } from './skeleton-lines.component';

describe('SkeletonLinesComponent', () => {
  let component: SkeletonLinesComponent;
  let fixture: ComponentFixture<SkeletonLinesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SkeletonLinesComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SkeletonLinesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
