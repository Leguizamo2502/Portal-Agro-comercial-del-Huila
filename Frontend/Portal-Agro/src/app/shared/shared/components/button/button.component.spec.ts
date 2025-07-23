import { ComponentFixture, TestBed } from '@angular/core/testing';
import { CommonModule } from '@angular/common';
import { ButtonComponent } from './button.component';

describe('ButtonComponent', () => {
  let component: ButtonComponent;
  let fixture: ComponentFixture<ButtonComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ButtonComponent, CommonModule] // Incluye CommonModule para los tests
    })
    .compileComponents();

    fixture = TestBed.createComponent(ButtonComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should have default values', () => {
    expect(component.type).toBe('button');
    expect(component.disabled).toBe(false);
    expect(component.class).toBe('');
    expect(component.variant).toBe('primary');
  });

  it('should apply correct CSS classes', () => {
    const compiled = fixture.nativeElement;
    const button = compiled.querySelector('button');
    
    expect(button.classList).toContain('btn');
    expect(button.classList).toContain('primary');
  });

  it('should be disabled when disabled input is true', () => {
    component.disabled = true;
    fixture.detectChanges();
    
    const compiled = fixture.nativeElement;
    const button = compiled.querySelector('button');
    
    expect(button.disabled).toBe(true);
  });
});