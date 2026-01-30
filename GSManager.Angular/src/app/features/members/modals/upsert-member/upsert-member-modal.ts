import { Component, inject, input, output, computed } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { ModalBaseComponent } from '../../../../shared/components/modal-base';
import { ButtonComponent } from '../../../../shared/components/button/button';
import { MemberDto} from '../../../../shared/models';
import { CommonModule } from '@angular/common';
import { PlotService, PriviledgeService, RoleService } from '../../../../core/services';
import { SearchableSelectComponent } from '../../../../shared/components';
import { ManyPlotSelectComponent } from './components/many-plot-select';

@Component({
  selector: 'app-upsert-member-modal',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, ModalBaseComponent, ButtonComponent, SearchableSelectComponent, ManyPlotSelectComponent],
  templateUrl: './upsert-member-modal.html',
  styleUrls: ['./upsert-member-modal.scss']
})
export class UpsertMemberModalComponent {
  private readonly fb = inject(FormBuilder);
  private readonly priviledgeService = inject(PriviledgeService);
  private readonly roleService = inject(RoleService);
  private readonly plotService = inject(PlotService);

  readonly priviledgeOptions = this.priviledgeService.privelegeSelectList;
  readonly roleOptions = this.roleService.roleSelectList;
  readonly plotOptions = this.plotService.plotSelectList;

  readonly isPriviledgesLoading = computed(() => this.priviledgeService.loading());
  readonly isRolesLoading = computed(() => this.roleService.loading());
  readonly isPlotsLoading = computed(() => this.plotService.loading());

  readonly member = input<MemberDto | null>(null);
  readonly close = output<void>();
  readonly save = output<any>();

  readonly form = this.fb.group({
    firstName: ['', Validators.required],
    middleName: '',
    lastName: ['', Validators.required],
    phoneNumber: '',
    email: '',
    roleId: '',
    priviledgeId: '',
    plotIds: [[] as string[]]
  });

  async ngOnInit(): Promise<void> {
    const tasks = [];
    tasks.push(this.priviledgeService.getSelectList());
    tasks.push(this.roleService.getSelectList());
    tasks.push(this.plotService.getSelectList());

    const member = this.member();
    if (member) {
      this.form.patchValue({
        firstName: member.firstName,
        middleName: member.middleName,
        lastName: member.lastName,
        phoneNumber: member.phoneNumber,
        email: member.email,
        roleId: member.roleId || '',
        priviledgeId: member.priviledgeId || '',
        plotIds: member.plotIds || []
      });
    }

    await Promise.all(tasks);
  }

  onSave(): void {
    if (this.form.valid) {
      const formValue = this.form.value;

      this.save.emit({ id: this.member()?.id ?? null, ...formValue });
    } else {
      this.form.markAllAsTouched();
    }
  }
}
