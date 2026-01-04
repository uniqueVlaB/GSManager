import { Component, ChangeDetectionStrategy, inject, signal, output } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { CreateMemberDto } from '../../../../shared/models';
import { ModalBaseComponent, ButtonComponent } from '../../../../shared/components';

@Component({
  selector: 'app-add-member-modal',
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './add-member-modal.html',
  styleUrl: './add-member-modal.scss',
  imports: [ReactiveFormsModule, ModalBaseComponent, ButtonComponent]
})
export class AddMemberModalComponent {
  private readonly fb = inject(FormBuilder);

  readonly close = output<void>();
  readonly submit = output<CreateMemberDto>();

  readonly form = this.createForm();
  readonly isSubmitting = signal(false);

  private createForm(): FormGroup {
    return this.fb.group({
      firstName: ['', [Validators.required, Validators.minLength(2)]],
      middleName: [''],
      lastName: ['', [Validators.required, Validators.minLength(2)]],
      email: ['', [Validators.required, Validators.email]],
      phoneNumber: ['']
    });
  }

  closeModal(): void {
    this.close.emit();
  }

  onSubmit(): void {
    if (this.form.invalid) {
      this.markFormGroupTouched(this.form);
      return;
    }

    this.isSubmitting.set(true);
    const formValue = this.form.value;

    const memberData: CreateMemberDto = {
      firstName: formValue.firstName,
      middleName: formValue.middleName || null,
      lastName: formValue.lastName,
      email: formValue.email,
      phoneNumber: formValue.phoneNumber || null,
      roleId: null,
      priviledgeId: null,
      plotIds: null
    };

    this.submit.emit(memberData);
    // Reset loading state after emitting
    setTimeout(() => this.isSubmitting.set(false), 500);
  }

  private markFormGroupTouched(formGroup: FormGroup): void {
    Object.keys(formGroup.controls).forEach(key => {
      const control = formGroup.get(key);
      control?.markAsTouched();
    });
  }
}
