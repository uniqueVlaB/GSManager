import { Component, inject, signal } from '@angular/core';
import { SettingPicker } from "./setting-picker/setting-picker";
import { SettingSwitch } from "./setting-switch/setting-switch";
import { Theme, ThemeService } from '../../core/services';

@Component({
  selector: 'app-settings',
  imports: [SettingPicker, SettingSwitch],
  templateUrl: './settings.html',
  styleUrl: './settings.scss',
})
export class SettingsComponent {
  readonly themeOptions = [
    { value: 'light', label: 'Light' },
    { value: 'dark', label: 'Dark' },
    { value: 'system', label: 'Follow browser' }
  ];

  readonly themeService = inject(ThemeService);

  readonly notificationsEnabled = signal(false);

  onThemeSelected(option: string) {
    this.themeService.setTheme(option as Theme);
  }
}
