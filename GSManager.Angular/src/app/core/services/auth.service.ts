import { computed, inject, Injectable, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { firstValueFrom } from 'rxjs';
import { jwtDecode } from 'jwt-decode';
import { environment } from '../../../environments/environment';
import { AuthRequest, AuthResponse } from '../../shared/models/auth.model';
import { AppPermission } from '../../shared/enums/app-permission.enum';
import { ToastService } from './toast.service';

/** Shape of the JWT payload returned by the GSManager API. */
interface GsmJwtPayload {
  sub?: string;
  jti?: string;
  'http://schemas.microsoft.com/ws/2008/06/identity/claims/role'?: string;
  permission?: string | string[];
  exp?: number;
  iss?: string;
  aud?: string;
}

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private readonly http = inject(HttpClient);
  private readonly toastService = inject(ToastService);
  private readonly apiUrl = `${environment.apiUrl}/api/auth`;

  // State signals
  private readonly isAuthenticatedSignal = signal(false);
  private readonly userRoleSignal = signal<string | null>(null);
  private readonly usernameSignal = signal<string | null>(null);
  private readonly permissionsSignal = signal<string[] | null>(null);
  // Token is stored in memory only
  private readonly tokenSignal = signal<string | null>(null);
  private readonly loadingSignal = signal(false);
  private sessionRestorePromise: Promise<void> | null = null;

  // Public readonly signals
  readonly userRole = this.userRoleSignal.asReadonly();
  readonly username = this.usernameSignal.asReadonly();
  readonly permissions = this.permissionsSignal.asReadonly();
  readonly isAuthenticated = this.isAuthenticatedSignal.asReadonly();
  readonly token = this.tokenSignal.asReadonly();
  readonly loading = this.loadingSignal.asReadonly();

  /** True when the user holds the `full_access` permission. */
  readonly hasFullAccess = computed(() => this.permissionsSignal()?.includes(AppPermission.FullAccess) ?? false);

  /**
   * Returns `true` if the user owns **any** of the provided permissions,
   * or if the user has `full_access` (which implies all permissions).
   */
  hasPermission(...required: AppPermission[]): boolean {
    const current = this.permissionsSignal();
    if (!current) return false;
    if (current.includes(AppPermission.FullAccess)) return true;
    return required.some(permission => current.includes(permission));
  }

  /**
   * Attempts to restore the session by refreshing the access token.
   * Only makes one HTTP call – subsequent calls reuse the same promise.
   * Called by route guards before checking authentication state.
   */
  tryRestoreSession(): Promise<void> {
    if (!this.sessionRestorePromise) {
      this.sessionRestorePromise = this.refreshAccessToken(false);
    }
    return this.sessionRestorePromise;
  }

  async login(authRequest: AuthRequest): Promise<void> {
    this.loadingSignal.set(true);
    try {
      // The backend must set the Refresh Token in an HttpOnly cookie
      const authResponse = await firstValueFrom(
        this.http.post<AuthResponse>(`${this.apiUrl}/login`, authRequest, {
          withCredentials: true 
        })
      );
      
      this.setSession(authResponse.accessToken);
      this.toastService.success('Login successful');
    } catch (err) {
      console.error('Login failed:', err);
      this.toastService.error('Login failed. Please check your credentials.');
    } finally {
      this.loadingSignal.set(false);
    }
  }

  async refreshAccessToken(showErrorToast = true): Promise<void> {
    try {
      this.loadingSignal.set(true);
      const response = await firstValueFrom(
        this.http.post<AuthResponse>(`${this.apiUrl}/refresh-token`, {}, {
          withCredentials: true 
        })
      );
      this.setSession(response.accessToken);
    } catch (error) {
      this.clearSession();
      console.error('Token refresh failed:', error);
      if (showErrorToast) {
        this.toastService.error('Session expired. Please log in again.');
      }
    } finally {
      this.loadingSignal.set(false);
    }
  }

  async logout(showToast = true): Promise<void> {
    try {
      // Call logout endpoint to clear the HttpOnly cookie
      await firstValueFrom(this.http.post(`${this.apiUrl}/logout`, {}, { withCredentials: true }));
    } catch (e) {
      // Ignore errors on logout
    } finally {
      this.clearSession();
      if (showToast) {
        this.toastService.info('Logged out successfully');
      }
    }
  }

  async getAccessToken(): Promise<string | null> {
    const currentToken = this.tokenSignal();
    if (!currentToken || this.isTokenExpired(currentToken)) {
      await this.refreshAccessToken();
    }
    return this.tokenSignal();
  }

  isTokenExpired(token: string): boolean {
    try {
      const decoded: any = jwtDecode(token);
      if (!decoded.exp) return true;
      
      const currentTime = Date.now() / 1000;
      return decoded.exp < currentTime;
    } catch (error) {
      return true;
    }
  }

  private setSession(token: string): void {
    this.tokenSignal.set(token);
    this.isAuthenticatedSignal.set(true);

    try {
      const decoded = jwtDecode<GsmJwtPayload>(token);
      this.usernameSignal.set(decoded.sub ?? null);
      this.userRoleSignal.set(
        decoded['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'] ?? null
      );
      
      let permissions: string[] | null = null;
      if (decoded.permission) {
        permissions = Array.isArray(decoded.permission) ? decoded.permission : [decoded.permission];
      }
      this.permissionsSignal.set(permissions);
    } catch {
      this.usernameSignal.set(null);
      this.userRoleSignal.set(null);
      this.permissionsSignal.set(null);
    }
  }

  private clearSession(): void {
    this.tokenSignal.set(null);
    this.isAuthenticatedSignal.set(false);
    this.userRoleSignal.set(null);
    this.usernameSignal.set(null);
    this.permissionsSignal.set(null);
  }
}

