import { ChangeDetectionStrategy, Component, computed, input, output } from '@angular/core';

export type ToggleVariant = 'primary' | 'success' | 'danger' | 'warning' | 'info';
export type ToggleSize = 'sm' | 'md' | 'lg';

let nextId = 0;

@Component({
  selector: 'app-toggle',
  templateUrl: './toggle.html',
  styleUrl: './toggle.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ToggleComponent {
  readonly checked = input<boolean>(false);
  readonly disabled = input<boolean>(false);
  readonly variant = input<ToggleVariant>('primary');
  readonly size = input<ToggleSize>('md');
  readonly labelText = input<string>('');

  readonly toggleId = `app-toggle-${++nextId}`;

  readonly checkedChange = output<boolean>();

  readonly trackClasses = computed(() =>
    ['toggle-track', `toggle-track--${this.variant()}`, `toggle-track--${this.size()}`].join(' ')
  );

  onChange(event: Event): void {
    const target = event.target as HTMLInputElement;
    this.checkedChange.emit(target.checked);
  }
}
