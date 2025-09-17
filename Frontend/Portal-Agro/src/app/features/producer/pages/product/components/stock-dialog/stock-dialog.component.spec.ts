import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StockDialogComponent } from './stock-dialog.component';

describe('StockDialogComponent', () => {
  let component: StockDialogComponent;
  let fixture: ComponentFixture<StockDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [StockDialogComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(StockDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
