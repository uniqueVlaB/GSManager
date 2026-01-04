import { Component, ChangeDetectionStrategy, OnInit, inject, signal, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MemberService, PlotService, PriviledgeService } from '../../../core/services';
import { ButtonComponent, ModalBaseComponent } from '../../../shared/components';
import { FullPlotDto } from '../../../shared/models';

@Component({
  selector: 'app-plot-list',
  standalone: true,
  imports: [CommonModule, ButtonComponent, ModalBaseComponent],
  templateUrl: './plot-list.html',
  styleUrl: './plot-list.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class PlotListComponent implements OnInit {
  private readonly plotService = inject(PlotService);
  private readonly memberService = inject(MemberService);
  private readonly priviledgeService = inject(PriviledgeService);

  readonly plotDetailsModalIsOpened = signal(false);
  
  readonly plots = this.plotService.plots;
  readonly plotOwners = this.memberService.members;
  readonly plotPriviledges = this.priviledgeService.priveleges;

  readonly loading = computed(() => 
    this.plotService.loading() || this.memberService.loading() || this.priviledgeService.loading());

  readonly fullPlots = computed(() => {
    const currentPlots = this.plots();
    const currentOwners = this.plotOwners();
    const currentPriviledges = this.plotPriviledges();

    let fullPlots: FullPlotDto[] = [];
    for(let plot of currentPlots) 
      {
        const owner = plot.ownerId 
          ? currentOwners.find(m => m.id === plot.ownerId)
          : null;
        const priviledge = plot.priviledgeId
          ? currentPriviledges.find(p => p.id === plot.priviledgeId)
          : null;

        fullPlots.push({
          ...plot,
          owner,
          priviledge
        });
      }
    return fullPlots;
  });

  async ngOnInit(): Promise<void> {
    await this.loadData();
  }

  async refresh(): Promise<void> {
    await this.loadData();
  }

  private async loadData(): Promise<void> {
    await this.plotService.loadPlots();
    
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
        tasks.push(this.memberService.loadMembers({ ids: ownerIds }));
      }
      if (priviledgeIds.length > 0) {
        tasks.push(this.priviledgeService.loadPriviledges({ ids: priviledgeIds }));
      }

      await Promise.all(tasks);
    }


  }

  onPlotDblClick(): void {
    this.plotDetailsModalIsOpened.set(true);
  }

  closePlotDetailsModal(): void {
    this.plotDetailsModalIsOpened.set(false);
  }


}