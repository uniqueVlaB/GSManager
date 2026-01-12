import { Injectable, inject, signal, computed } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { firstValueFrom } from 'rxjs';
import { environment } from '../../../environments/environment';
import { PlotDto, CreatePlotDto, PlotQueryParams } from '../../shared/models';
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
  private readonly plotsSignal = signal<PlotDto[]>([]);
  private readonly loadingSignal = signal(false);

  // Public readonly signals
  readonly plots = this.plotsSignal.asReadonly();
  readonly loading = this.loadingSignal.asReadonly();

  // Derived state
  readonly totalCount = computed(() => this.plotsSignal().length);

  async loadPlots(params?: PlotQueryParams): Promise<void> {
    this.loadingSignal.set(true);
    const httpParams = HttpUtils.createParams(params);

    try {
      const plots = await firstValueFrom(this.http.get<PlotDto[]>(this.apiUrl, { params: httpParams }));
      this.plotsSignal.set(plots);
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
      this.plotsSignal.update(plots => [...plots, newPlot]);
      this.toastService.success(`Plot ${newPlot.number} created successfully`);
    } catch (err) {
      console.error('Failed to create plot:', err);
      this.toastService.error('Failed to create plot');
    } finally {
      this.loadingSignal.set(false);
    }
  }
  
  deletePlot(plotId: string): void {
    this.loadingSignal.set(true);
    this.http.delete(`${this.apiUrl}/${plotId}`).subscribe({
      next: () => {
        this.plotsSignal.update(plots => plots.filter(p => p.id !== plotId));
        this.toastService.success('Plot deleted successfully');
        this.loadingSignal.set(false);
      },
      error: (err) => {
        console.error('Failed to delete plot:', err);
        this.toastService.error('Failed to delete plot');
        this.loadingSignal.set(false);
      }
    });
  }

  async updatePlot(id: string, plot: CreatePlotDto): Promise<void> {
    this.loadingSignal.set(true);
    try {
      const updatedPlot = await firstValueFrom(this.http.put<PlotDto>(`${this.apiUrl}/${id}`, plot));
      this.plotsSignal.update(plots => plots.map(p => p.id === id ? updatedPlot : p));
      this.toastService.success(`Plot ${updatedPlot.number} updated successfully`);
    } catch (err) {
      console.error('Failed to update plot:', err);
      this.toastService.error('Failed to update plot');
    } finally {
      this.loadingSignal.set(false);
    }
  }
}
