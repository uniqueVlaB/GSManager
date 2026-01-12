import { Component, effect, inject, input, output, computed } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { ModalBaseComponent } from '../../../../shared/components/modal-base';
import { ButtonComponent } from '../../../../shared/components/button/button';
import { FullPlotDto} from '../../../../shared/models';
import { CommonModule } from '@angular/common';
import { MemberService, PriviledgeService } from '../../../../core/services';
import { SearchableSelectComponent } from '../../../../shared/components';

@Component({
  selector: 'app-upsert-plot-modal',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, ModalBaseComponent, ButtonComponent, SearchableSelectComponent],
  templateUrl: './upsert-plot-modal.html',
  styleUrls: ['./upsert-plot-modal.scss']
})
export class UpsertPlotModalComponent {
  private readonly fb = inject(FormBuilder);
  private readonly memberService = inject(MemberService);
  private readonly priviledgeService = inject(PriviledgeService);

  readonly ownerOptions = this.memberService.memberSelectList;
  readonly priviledgeOptions = this.priviledgeService.privelegeSelectList;

  readonly isOwnersLoading = computed(() => this.memberService.loading());
  readonly isPriviledgesLoading = computed(() => this.priviledgeService.loading());

  readonly plot = input<FullPlotDto | null>(null);
  readonly close = output<void>();
  readonly save = output<any>();

  readonly form = this.fb.group({
    number: ['', Validators.required],
    square: [null as number | null],
    description: [''],
    cadastreNumber: [''],
    ownerId: [''],
    priviledgeId: ['']
  });

  async ngOnInit(): Promise<void> {
    const tasks = [];
    tasks.push(this.memberService.getMemberSelectList());
    tasks.push(this.priviledgeService.getPriviledgeSelectList());
    await Promise.all(tasks);

    const p = this.plot();
    if (p) {
      this.form.patchValue({
        number: p.number,
        square: p.square ?? null,
        description: p.description ?? null,
        cadastreNumber: p.cadastreNumber ?? null,
        ownerId:  p.ownerId ?? null,
        priviledgeId: p.priviledgeId ?? null
      });
    }
  }

  onSave(): void {
    if (this.form.valid) {
      const formValue = this.form.value;

      this.save.emit(formValue);
    } else {
      this.form.markAllAsTouched();
    }
  }
}
