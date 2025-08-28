import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CategoryPillComponent } from './category-pill.component';

describe('CategoryPillComponent', () => {
  let component: CategoryPillComponent;
  let fixture: ComponentFixture<CategoryPillComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CategoryPillComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CategoryPillComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
