import { Component, input, output } from "@angular/core";
import { ModalBaseComponent } from "../modal-base";
import { ButtonComponent } from "../button/button";

@Component({
  selector: "app-confirmation-modal",
  templateUrl: "./confirmation-modal.html",
  styleUrls: ["./confirmation-modal.scss"],
  imports: [ModalBaseComponent, ButtonComponent]
})
export class ConfirmationModalComponent {
  readonly title = input<string>('Confirm Action');
  readonly message = input.required<string>();
  readonly confirmLabel = input('Confirm');
  readonly cancelLabel = input('Cancel');

  readonly close = output<void>();
  readonly confirm = output<void>();

  onConfirm(): void {
    this.confirm.emit();
  }

  onCancel(): void {
    this.close.emit();
  }
}