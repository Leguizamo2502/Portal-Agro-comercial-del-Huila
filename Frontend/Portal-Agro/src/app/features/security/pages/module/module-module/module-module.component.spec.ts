import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ModuleModuleComponent } from '../module-module/module-module.component';

describe('ModuleModuleComponent', () => {
  let component: ModuleModuleComponent;
  let fixture: ComponentFixture<ModuleModuleComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ModuleModuleComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ModuleModuleComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
