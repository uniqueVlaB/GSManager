import { Component, ChangeDetectionStrategy } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { AnonimousHeaderComponent } from "./header/header";

@Component({
  selector: 'app-anonimous-layout',
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [RouterOutlet, AnonimousHeaderComponent],
  styleUrl: './anonimous-layout.scss',
  template: `
    <div class="anonimous-layout">
      <app-anonimous-header />
      <router-outlet />
    </div>
  `
})
export class AnonimousLayoutComponent {}
