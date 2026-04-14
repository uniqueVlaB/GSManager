import { Injectable, signal, effect, inject, PLATFORM_ID } from '@angular/core';
import { isPlatformBrowser } from '@angular/common';

export type Theme = 'light' | 'dark' | 'system';

@Injectable({
  providedIn: 'root'
})
export class ThemeService {
  private platformId = inject(PLATFORM_ID);
  theme = signal<Theme>('system');

  constructor() {
    if (isPlatformBrowser(this.platformId)) {
      const savedTheme = localStorage.getItem('theme') as Theme;
      if (savedTheme) {
        this.theme.set(savedTheme);
      }

      const mediaQuery = window.matchMedia('(prefers-color-scheme: dark)');

      const applyTheme = (theme: Theme) => {
        const resolved = theme === 'system'
          ? (mediaQuery.matches ? 'dark' : 'light')
          : theme;
        document.documentElement.classList.remove('light', 'dark');
        document.documentElement.classList.add(resolved);
      };

      effect(() => {
        const currentTheme = this.theme();
        localStorage.setItem('theme', currentTheme);
        applyTheme(currentTheme);
      });

      mediaQuery.addEventListener('change', () => {
        if (this.theme() === 'system') {
          applyTheme('system');
        }
      });
    }
  }

  setTheme(theme: Theme) {
    this.theme.set(theme);
  }

  toggleTheme() {
    this.theme.update(t => t === 'light' ? 'dark' : t === 'dark' ? 'system' : 'light');
  }
}
