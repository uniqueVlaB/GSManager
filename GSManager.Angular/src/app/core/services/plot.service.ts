import { Injectable, inject, signal, computed } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { firstValueFrom } from 'rxjs';
import { environment } from '../../../environments/environment';
import { PlotDto, CreatePlotDto, PlotQueryParams, PaginatedResponse, SelectListItem } from '../../shared/models';
import { ToastService } from './toast.service';
import { HttpUtils } from '../utils';

@Injectable({
  providedIn: 'root'
})
export class PlotService {
  private readonly http = inject(HttpClient);
  private readonly toastService = inject(ToastService);
  private readonly apiUrl = `${environment.apiUrl}/plots`;

  // State signals
  private readonly pagedPlotsSignal = signal<PaginatedResponse<PlotDto> | null>(null);
  private readonly plotSelectListSignal = signal<SelectListItem[]>([]);
  private readonly loadingSignal = signal(false);

  // Public readonly signals
  readonly plots = computed(() => this.pagedPlotsSignal()?.items || []);
  readonly plotSelectList = this.plotSelectListSignal.asReadonly();
  readonly pagedPlots = this.pagedPlotsSignal.asReadonly();
  readonly loading = this.loadingSignal.asReadonly();

  // Derived state
  readonly totalCount = computed(() => this.pagedPlotsSignal()?.totalCount ?? 0);

  async getPlots(params?: PlotQueryParams): Promise<void> {
    this.loadingSignal.set(true);
    const httpParams = HttpUtils.createParams(params);

    try {
      const plots = await firstValueFrom(this.http.get<PaginatedResponse<PlotDto>>(this.apiUrl, { params: httpParams }));
      this.pagedPlotsSignal.set(plots);
    } catch (err) {
      console.error('Failed to load plots:', err);
      this.toastService.error('Failed to load plots');
    } finally {
      this.loadingSignal.set(false);
    }
  }

  async addPlot(plot: CreatePlotDto): Promise<void> {
    this.loadingSignal.set(true); 
    try {
      const newPlot = await firstValueFrom(this.http.post<PlotDto>(this.apiUrl, plot));
      this.pagedPlotsSignal.update(paginated => {
        if (!paginated) return null;
        return {
          ...paginated,
          items: [newPlot, ...paginated.items],
          totalCount: paginated.totalCount + 1
        };
      });
      this.toastService.success(`Plot ${newPlot.number} created successfully`);
    } catch (err) {
      console.error('Failed to create plot:', err);
      this.toastService.error('Failed to create plot');
    } finally {
      this.loadingSignal.set(false);
    }
  }
  
  async deletePlot(plotId: string): Promise<void> {
    this.loadingSignal.set(true);
    try {
      await firstValueFrom(this.http.delete(`${this.apiUrl}/${plotId}`));
      this.pagedPlotsSignal.update(paginated => {
        if (!paginated) return null;
        return {
          ...paginated,
          items: paginated.items.filter(p => p.id !== plotId),
          totalCount: paginated.totalCount - 1
        };
      });
      this.toastService.success('Plot deleted successfully');
    } catch (err) {
      console.error('Failed to delete plot:', err);
      this.toastService.error('Failed to delete plot');
    } finally {
      this.loadingSignal.set(false);
    }
  }

  async updatePlot(id: string, dto: CreatePlotDto): Promise<void> {
    this.loadingSignal.set(true);
    try {
       const updatedPlot = await firstValueFrom(this.http.put<PlotDto>(`${this.apiUrl}/${id}`, dto));
       this.pagedPlotsSignal.update(paginated => {
         if (!paginated) return paginated;
         return {
           ...paginated,
           items: paginated.items.map(p => p.id === id ? updatedPlot : p)
         };
       });
      this.toastService.success(`Plot ${updatedPlot.number} updated successfully`);
    } catch (err) {
      console.error('Failed to update plot:', err);
      this.toastService.error('Failed to update plot');
    } finally {
      this.loadingSignal.set(false);
    }
  }

  async getSelectList(): Promise<void> {
    this.loadingSignal.set(true);
    try {
      const plots = await firstValueFrom(this.http.get<SelectListItem[]>(`${this.apiUrl}/select-list`));
      this.plotSelectListSignal.set(plots);
    } catch (err) {
      console.error('Failed to load plot select list:', err);
      this.toastService.error('Failed to load plot select list');
    } finally {
      this.loadingSignal.set(false);
    }  
  }
}
