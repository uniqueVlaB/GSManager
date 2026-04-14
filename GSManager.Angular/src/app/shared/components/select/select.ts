import { ChangeDetectionStrategy, Component, input, output } from '@angular/core';

export interface SelectOption {
  value: string;
  label: string;
}

let nextId = 0;

@Component({
  selector: 'app-select',
  templateUrl: './select.html',
  styleUrl: './select.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class SelectComponent {
  readonly options = input<SelectOption[]>([]);
  readonly value = input<string>('');
  readonly placeholder = input<string>('');
  readonly disabled = input<boolean>(false);
  readonly labelText = input<string>('');

  readonly selectId = `app-select-${++nextId}`;

  readonly selectionChange = output<string>();

  onChange(event: Event): void {
    const target = event.target as HTMLSelectElement;
    this.selectionChange.emit(target.value);
  }
}
