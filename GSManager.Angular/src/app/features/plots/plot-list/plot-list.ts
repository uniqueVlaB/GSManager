import { Component, ChangeDetectionStrategy, OnInit, inject, signal, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MemberService, PlotService, PriviledgeService } from '../../../core/services';
import { ButtonComponent} from '../../../shared/components';
import { FullPlotDto } from '../../../shared/models';
import { PlotDetailsModalComponent, UpsertPlotModalComponent} from "../modals";
import { readonly } from '@angular/forms/signals';

@Component({
  selector: 'app-plot-list',
  standalone: true,
  imports: [CommonModule, ButtonComponent, PlotDetailsModalComponent, UpsertPlotModalComponent],
  templateUrl: './plot-list.html',
  styleUrl: './plot-list.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class PlotListComponent implements OnInit {
  private readonly plotService = inject(PlotService);
  private readonly memberService = inject(MemberService);
  private readonly priviledgeService = inject(PriviledgeService);

  readonly isPlotDetailsModalOpen = signal(false);
  readonly isUpsertPlotModalOpen = signal(false);

  readonly plots = this.plotService.plots;
  readonly plotOwners = this.memberService.members;
  readonly plotPriviledges = this.priviledgeService.priveleges;

  readonly loading = computed(() =>
    this.plotService.loading() || this.memberService.loading() || this.priviledgeService.loading());

  readonly fullPlots = computed<FullPlotDto[]>(() => {
    const currentPlots = this.plots();
    const currentOwners = this.plotOwners();
    const currentPriviledges = this.plotPriviledges();

    return currentPlots.map(plot => {
      const owner = plot.ownerId
        ? currentOwners.find(m => m.id === plot.ownerId) ?? null
        : null;
      const priviledge = plot.priviledgeId
        ? currentPriviledges.find(p => p.id === plot.priviledgeId) ?? null
        : null;

      return {
        ...plot,
        owner,
        priviledge
      };
    });
  });


  readonly selectedPlotId = signal<string | null>(null);

  readonly selectedPlot = computed<FullPlotDto | undefined>(() => {
    const plotId = this.selectedPlotId();
    if (!plotId) null;
    return this.fullPlots().find(p => p.id === plotId);
  });

  async ngOnInit(): Promise<void> {
    await this.loadData();
  }

  private async loadData(): Promise<void> {
    await this.plotService.getPlots();

    const currentPlots = this.plots();
    if (currentPlots.length > 0) {
      const ownerIds = [...new Set(currentPlots
        .map(plot => plot.ownerId)
        .filter((id): id is string => id != null))];

      const priviledgeIds = [...new Set(currentPlots
        .map(plot => plot.priviledgeId)
        .filter((id): id is string => id != null))];

      const tasks = [];
      if (ownerIds.length > 0) {
        tasks.push(this.memberService.getMembers({ ids: ownerIds }));
      }
      if (priviledgeIds.length > 0) {
        tasks.push(this.priviledgeService.getPriviledges({ ids: priviledgeIds }));
      }

      await Promise.all(tasks);
    }
  }

  onPlotDblClick(plotId: string): void {
    this.selectedPlotId.set(plotId);
    this.isPlotDetailsModalOpen.set(true);
  }

  closePlotDetailsModal(): void {
    this.isPlotDetailsModalOpen.set(false);
  }

  // Opens modal for CREATION (Upsert with no ID)
  openAddModal(): void {
    this.selectedPlotId.set(null); // Clear selection
    this.isUpsertPlotModalOpen.set(true);
  }

  // Opens modal for EDITING (Upsert with ID)
  openEditModal(): void {
    this.isPlotDetailsModalOpen.set(false);
    this.isUpsertPlotModalOpen.set(true);
  }

  async onUpsertSave(formData: any): Promise<void> {
    const plotId = this.selectedPlotId();

    if (plotId) {
      // UPDATE mode
      await this.plotService.updatePlot(plotId, formData);
    } else {
      // CREATE mode
      await this.plotService.addPlot(formData);
    }
    
    this.isUpsertPlotModalOpen.set(false);
    await this.loadData();
  }

  closeUpsertModal(): void {
    this.isUpsertPlotModalOpen.set(false);
    // If we have a selected plot (Edit mode cancel), go back to details
    if (this.selectedPlotId()) {
      this.isPlotDetailsModalOpen.set(true);
    }
  }

  deleteSelectedPlot(): void {
    const plotId = this.selectedPlotId();
    if (!plotId) return;
    this.plotService.deletePlot(plotId);
    this.isPlotDetailsModalOpen.set(false);
  }

  openAddPlotModal(): void {
    this.selectedPlotId.set(null);
    this.isUpsertPlotModalOpen.set(true);
  }

  async onAddPlotSave(plotData: any): Promise<void> {
    this.isUpsertPlotModalOpen.set(false);
    await this.plotService.addPlot(plotData);
    this.loadData();
  }

  closeAddPlotModal(): void {
    this.isUpsertPlotModalOpen.set(false);
  }
}