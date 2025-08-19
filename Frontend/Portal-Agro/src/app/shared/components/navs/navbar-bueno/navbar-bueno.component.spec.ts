import { ComponentFixture, TestBed } from '@angular/core/testing';

import { NavbarBuenoComponent } from './navbar-bueno.component';

describe('NavbarBuenoComponent', () => {
  let component: NavbarBuenoComponent;
  let fixture: ComponentFixture<NavbarBuenoComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [NavbarBuenoComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(NavbarBuenoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
