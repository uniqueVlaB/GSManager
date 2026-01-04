import { Injectable, inject, signal, computed } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { firstValueFrom } from 'rxjs';
import { environment } from '../../../environments/environment';
import { ToastService } from './toast.service';
import { PriviledgeDto, PriviledgeQueryParams } from '../../shared/models/priviledge.model';
import { HttpUtils } from '../utils';

@Injectable({
  providedIn: 'root'
})
export class PriviledgeService {
  private readonly http = inject(HttpClient);
  private readonly toastService = inject(ToastService);
  private readonly apiUrl = `${environment.apiUrl}/priviledges`;

  // State signals
  private readonly privelegesSignal = signal<PriviledgeDto[]>([]);
  private readonly loadingSignal = signal(false);

  // Public readonly signals
  readonly priveleges = this.privelegesSignal.asReadonly();
  readonly loading = this.loadingSignal.asReadonly();

  // Derived state
  readonly totalCount = computed(() => this.privelegesSignal().length);

  async loadPriviledges(params?: PriviledgeQueryParams): Promise<void> {
    this.loadingSignal.set(true);
    try {
      const httpParams = HttpUtils.createParams(params);
      const priviledges = await firstValueFrom(this.http.get<PriviledgeDto[]>(this.apiUrl, { params: httpParams }));
      this.privelegesSignal.set(priviledges);
    } catch (err) {
      console.error('Failed to load priviledges:', err);
      this.toastService.error('Failed to load priviledges');
    } finally {
      this.loadingSignal.set(false);
    }
  }
}
