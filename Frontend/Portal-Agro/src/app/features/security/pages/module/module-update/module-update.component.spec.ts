import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ModuleUpdateComponent } from './module-update.component';

describe('ModuleUpdateComponent', () => {
  let component: ModuleUpdateComponent;
  let fixture: ComponentFixture<ModuleUpdateComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ModuleUpdateComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ModuleUpdateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
