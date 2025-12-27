import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ButtonComponent } from './button';
import { By } from '@angular/platform-browser';

describe('ButtonComponent', () => {
  let component: ButtonComponent;
  let fixture: ComponentFixture<ButtonComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ButtonComponent]
    }).compileComponents();

    fixture = TestBed.createComponent(ButtonComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should apply primary variant by default', () => {
    const button = fixture.debugElement.query(By.css('button'));
    expect(button.classes['btn--primary']).toBe(true);
  });

  it('should apply correct variant when input is set', () => {
    fixture.componentRef.setInput('variant', 'secondary');
    fixture.detectChanges();
    const button = fixture.debugElement.query(By.css('button'));
    expect(button.classes['btn--secondary']).toBe(true);
  });

  it('should apply correct size when input is set', () => {
    fixture.componentRef.setInput('size', 'lg');
    fixture.detectChanges();
    const button = fixture.debugElement.query(By.css('button'));
    expect(button.classes['btn--lg']).toBe(true);
  });

  it('should apply full-width class when fullWidth is true', () => {
    fixture.componentRef.setInput('fullWidth', true);
    fixture.detectChanges();
    const button = fixture.debugElement.query(By.css('button'));
    expect(button.classes['btn--full-width']).toBe(true);
  });

  it('should be disabled when disabled input is true', () => {
    fixture.componentRef.setInput('disabled', true);
    fixture.detectChanges();
    const button = fixture.debugElement.query(By.css('button'));
    expect(button.nativeElement.disabled).toBe(true);
  });
});
