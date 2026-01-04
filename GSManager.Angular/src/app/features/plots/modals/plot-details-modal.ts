import { ChangeDetectionStrategy, Component, input, output} from "@angular/core";
import { FormBuilder } from "@angular/forms";
import { MemberDto, PlotDto } from "../../../shared/models";
import { PriviledgeDto } from "../../../shared/models/priviledge.model";

@Component({
  selector: 'app-plot-details-modal',
  standalone: true,
  template: `<p>Plot Details Modal Works!</p>`,
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class PlotDetailsModalComponent 
{
readonly plot = input.required<PlotDto>();
readonly member = input<MemberDto | null>();
readonly priviledge = input<PriviledgeDto | null>();

readonly close = output<void>();
readonly edit = output<void>();


}