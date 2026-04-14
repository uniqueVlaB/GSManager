import { ChangeDetectionStrategy, Component, input, output } from '@angular/core';
import { ToggleComponent } from '../../../shared/components/toggle/toggle';

@Component({
  selector: 'app-setting-switch',
  imports: [ToggleComponent],
  templateUrl: './setting-switch.html',
  styleUrl: './setting-switch.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class SettingSwitch {
  readonly checked = input<boolean>(false);
  readonly title = input('setting');
  readonly description = input('setting description');

  readonly checkedChange = output<boolean>();
}
