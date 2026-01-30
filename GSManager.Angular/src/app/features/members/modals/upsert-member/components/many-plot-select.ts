import { Component, computed, inject, input, output } from "@angular/core";
import { ButtonComponent, SearchableSelectComponent } from "../../../../../shared/components";
import { SelectListItem } from "../../../../../shared/models";
import {MatGridListModule} from '@angular/material/grid-list';

@Component({
  selector: 'many-plot-select',
  standalone: true,
  imports: [ButtonComponent, SearchableSelectComponent, MatGridListModule],
  templateUrl: './many-plot-select.html',
  styleUrls: ['./many-plot-select.scss']
})
export class ManyPlotSelectComponent {
readonly selectedPlotIds = input<string[]>([]);
readonly options = input<SelectListItem[]>([]);
readonly isLoading = input<boolean>(false);

readonly selectedPlotsUpdated = output<string[]>();


getLabelById(plotId: string): string {
  const plot = this.options().find(o => o.id.toLowerCase() === plotId.toLowerCase());
  let label = plot ? plot.label : plotId;
  return label;
}

removePlot(plotId: string): void {
  const updatedSelection = this.selectedPlotIds().filter(id => id.toLowerCase() !== plotId.toLowerCase());
  this.selectedPlotsUpdated.emit(updatedSelection);
}

onSelectionChange(selectedIds: string[]): void {
  this.selectedPlotsUpdated.emit(selectedIds);
}

addSelectedPlot(plotId: string | null): void {
  if (plotId && !this.selectedPlotIds().includes(plotId)) {
    const updatedSelection = [...this.selectedPlotIds(), plotId];
    this.selectedPlotsUpdated.emit(updatedSelection);
  }
}
}