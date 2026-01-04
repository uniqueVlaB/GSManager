import { Component, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { ToastListComponent } from './shared/components';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, ToastListComponent],
  templateUrl: './app.html',
  styleUrl: './app.scss'
})
export class App {
  protected readonly title = signal('GSManager.Angular');
}
