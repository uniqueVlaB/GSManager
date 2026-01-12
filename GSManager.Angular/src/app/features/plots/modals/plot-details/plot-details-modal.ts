import { ChangeDetectionStrategy, Component, input, output, signal} from "@angular/core";
import { FullPlotDto} from "../../../../shared/models";
import { ButtonComponent, ModalBaseComponent } from "../../../../shared/components";
import { ConfirmationModalComponent } from "../../../../shared/components";
import { RouterLink } from "@angular/router";

@Component({
  selector: 'app-plot-details-modal',
  standalone: true,
  templateUrl: './plot-details-modal.html',
  styleUrl: './plot-details-modal.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [ModalBaseComponent, ButtonComponent, ConfirmationModalComponent, RouterLink]
})
export class PlotDetailsModalComponent 
{
plot = input.required<FullPlotDto>();

readonly close = output<void>();
readonly delete = output<void>();
readonly edit = output<void>();

readonly showDeleteConfirmation = signal(false);

onDeleteClick(): void {
  this.showDeleteConfirmation.set(true);
}

onDeleteConfirm(): void {
  this.showDeleteConfirmation.set(false);
  this.delete.emit();
}

onDeleteCancel(): void {
  this.showDeleteConfirmation.set(false);
}

deletePlot(): void {
  this.delete.emit();
}
}