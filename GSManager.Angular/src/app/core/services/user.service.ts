import { inject, Injectable, signal } from "@angular/core";
import { environment } from "../../../environments/environment";
import { HttpClient } from "@angular/common/http";
import { ToastService } from "./toast.service";
import { UserInfo } from "../../shared/models/user.model";
import { firstValueFrom } from "rxjs";
import { AuthService } from "./auth.service";
import { HttpUtils } from "../utils";

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private readonly http = inject(HttpClient);
  private readonly toastService = inject(ToastService);
  private readonly authService = inject(AuthService);
  private readonly apiUrl = `${environment.apiUrl}/api/users`;

  private readonly loadingSignal = signal(false);
  private readonly userInfoSignal = signal<UserInfo | null>(null);

  readonly userInfo = this.userInfoSignal.asReadonly();
  readonly loading = this.loadingSignal.asReadonly();

  async getCurrentUserInfo(): Promise<void> {
    this.loadingSignal.set(true);
    try {
      const userInfo = await firstValueFrom(this.http.get<UserInfo>(`${this.apiUrl}/me`, { headers: HttpUtils.AddAuthHeader(await this.authService.getAccessToken() || '')}));
      this.userInfoSignal.set(userInfo);
    } catch (error) {
      this.toastService.error('Failed to load user info');
      console.error('Error fetching user info:', error);
    }
    finally {
      this.loadingSignal.set(false);
    }
  }
}