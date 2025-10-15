import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DesignDecorationComponent } from './design-decoration.component';

describe('DesignDecorationComponent', () => {
  let component: DesignDecorationComponent;
  let fixture: ComponentFixture<DesignDecorationComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DesignDecorationComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DesignDecorationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
