import { ChangeDetectionStrategy, Component, input, output } from '@angular/core';
import { SelectComponent, SelectOption } from '../../../shared/components/select/select';

@Component({
  selector: 'app-setting-picker',
  imports: [SelectComponent],
  templateUrl: './setting-picker.html',
  styleUrl: './setting-picker.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class SettingPicker {
  readonly options = input<SelectOption[]>([{ value: 'option', label: 'Option' }]);
  readonly selectedOptionValue = input<string>('option');
  readonly title = input('setting');
  readonly description = input('setting description');

  readonly pickedOption = output<string>();
}
