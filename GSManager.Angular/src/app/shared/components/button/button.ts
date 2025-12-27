import { Component, input, ChangeDetectionStrategy, computed } from '@angular/core';

export type ButtonVariant = 'primary' | 'secondary' | 'outline' | 'ghost' | 'danger';
export type ButtonSize = 'sm' | 'md' | 'lg';
export type ButtonType = 'button' | 'submit' | 'reset';

@Component({
  selector: 'app-button',
  imports: [],
  template: `
    <button
      [type]="type()"
      [class]="buttonClasses()"
      [disabled]="disabled()"
    >
      @if (icon()) {
        <span class="btn-icon">{{ icon() }}</span>
      }
      <ng-content />
    </button>
  `,
  styleUrls: ['./button.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ButtonComponent {
  variant = input<ButtonVariant>('primary');
  size = input<ButtonSize>('md');
  type = input<ButtonType>('button');
  icon = input<string>();
  disabled = input<boolean>(false);
  fullWidth = input<boolean>(false);
  customClass = input<string>('', { alias: 'class' });

  buttonClasses = computed(() => {
    const classes = [
      'btn',
      `btn--${this.variant()}`,
      `btn--${this.size()}`,
      this.customClass()
    ];

    if (this.fullWidth()) {
      classes.push('btn--full-width');
    }

    return classes.join(' ');
  });
}
