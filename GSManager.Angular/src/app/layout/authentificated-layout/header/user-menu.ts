import { ChangeDetectionStrategy, Component, inject, signal } from "@angular/core";
import { Router, RouterLink } from "@angular/router";
import { AuthService } from "../../../core/services";

@Component({
  selector: 'app-user-menu',
  imports: [RouterLink],
  styleUrl: './user-menu.scss',
  templateUrl: './user-menu.html',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class UserMenuComponent {
  readonly authService = inject(AuthService);
  private readonly router = inject(Router);
  
  isUserMenuOpen = signal(false);

  toggleUserMenu(): void {
    this.isUserMenuOpen.update(v => !v);
  }

  closeUserMenu(): void {
    this.isUserMenuOpen.set(false);
  }

  async logout(): Promise<void> {
    this.closeUserMenu();
    await this.authService.logout();
    this.router.navigate(['/auth/login']);
  }
}