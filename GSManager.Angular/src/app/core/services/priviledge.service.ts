import { Injectable, inject, signal, computed } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { firstValueFrom } from 'rxjs';
import { environment } from '../../../environments/environment';
import { ToastService } from './toast.service';
import { PriviledgeDto, PriviledgeQueryParams } from '../../shared/models/priviledge.model';
import { HttpUtils } from '../utils';
import { SelectListItem } from '../../shared/models';

@Injectable({
  providedIn: 'root'
})
export class PriviledgeService {
  private readonly http = inject(HttpClient);
  private readonly toastService = inject(ToastService);
  private readonly apiUrl = `${environment.apiUrl}/priviledges`;

  // State signals
  private readonly privelegesSignal = signal<PriviledgeDto[]>([]);
   private readonly privelegeSelectListSignal = signal<SelectListItem[]>([]);
  private readonly loadingSignal = signal(false);

  // Public readonly signals
  readonly priveleges = this.privelegesSignal.asReadonly();
  readonly privelegeSelectList = this.privelegeSelectListSignal.asReadonly();
  readonly loading = this.loadingSignal.asReadonly();

  // Derived state
  readonly totalCount = computed(() => this.privelegesSignal().length);

  async getPriviledges(params?: PriviledgeQueryParams): Promise<void> {
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

  async getPriviledgeSelectList(): Promise<void> {
    this.loadingSignal.set(true);
    try {
      const selectList = await firstValueFrom(this.http.get<SelectListItem[]>(`${this.apiUrl}/select-list`));
      this.privelegeSelectListSignal.set(selectList);
    } catch (err) {
      console.error('Failed to load priviledge select list:', err);
      this.toastService.error('Failed to load priviledge options');
    } finally {
      this.loadingSignal.set(false);
    }
  }
}
