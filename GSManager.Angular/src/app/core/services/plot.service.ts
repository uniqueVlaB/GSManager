import { Injectable, inject, signal, computed } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { firstValueFrom } from 'rxjs';
import { environment } from '../../../environments/environment';
import { PlotDto, CreatePlotDto } from '../../shared/models';
import { ToastService } from './toast.service';

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

  async loadPlots(): Promise<void> {
    this.loadingSignal.set(true);
    try {
      const plots = await firstValueFrom(this.http.get<PlotDto[]>(this.apiUrl));
      this.plotsSignal.set(plots);
    } catch (err) {
      console.error('Failed to load plots:', err);
      this.toastService.error('Failed to load plots');
    } finally {
      this.loadingSignal.set(false);
    }
  }

  addPlot(plot: CreatePlotDto): void {
    this.loadingSignal.set(true);
    this.http.post<PlotDto>(this.apiUrl, plot).subscribe({
      next: (newPlot) => {
        this.plotsSignal.update(plots => [...plots, newPlot]);
        this.toastService.success(`Plot ${newPlot.number} created successfully`);
        this.loadingSignal.set(false);
      },
      error: (err) => {
        console.error('Failed to create plot:', err);
        this.toastService.error('Failed to create plot');
        this.loadingSignal.set(false);
      }
    });
  }
}
