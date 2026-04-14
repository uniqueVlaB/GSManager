import { ChangeDetectionStrategy, Component, computed, inject, OnInit, signal } from "@angular/core";
import { ReactiveFormsModule, FormBuilder, Validators, AbstractControl, ValidationErrors } from "@angular/forms";
import { NgOptimizedImage } from "@angular/common";
import { AuthService, UserService } from "../../../core/services";
import { UserInfo } from "../../../shared/models/user.model";

function passwordsMatchValidator(control: AbstractControl): ValidationErrors | null {
  const pw = control.get('newPassword')?.value;
  const confirm = control.get('confirmPassword')?.value;
  return pw && confirm && pw !== confirm ? { passwordsMismatch: true } : null;
}

@Component({
  selector: 'app-profile',
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './profile.html',
  styleUrl: './profile.scss',
  imports: [ReactiveFormsModule, NgOptimizedImage]
})
export class ProfileComponent implements OnInit {
  private readonly fb = inject(FormBuilder);
  readonly userService = inject(UserService);

  readonly userInfo = this.userService.userInfo;

  readonly showCurrentPassword = signal(false);
  readonly showNewPassword = signal(false);
  readonly showConfirmPassword = signal(false);

  toggleCurrentPassword(): void {
    this.showCurrentPassword.update(v => !v);
  }

  toggleNewPassword(): void {
    this.showNewPassword.update(v => !v);
  }

  toggleConfirmPassword(): void {
    this.showConfirmPassword.update(v => !v);
  }

  readonly changePasswordForm = this.fb.nonNullable.group(
    {
      currentPassword: ['', [Validators.required, Validators.minLength(6)]],
      newPassword: ['', [Validators.required, Validators.minLength(6)]],
      confirmPassword: ['', Validators.required],
    },
    { validators: passwordsMatchValidator }
  );

  async ngOnInit(): Promise<void> {
    await this.userService.getCurrentUserInfo();
  }
}
