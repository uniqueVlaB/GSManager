import { Component, computed, ElementRef, forwardRef, HostListener, inject, input, output, signal, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ControlValueAccessor, NG_VALUE_ACCESSOR, FormsModule } from '@angular/forms';
import { SelectListItem } from '../../models';
import { readonly } from '@angular/forms/signals';


@Component({
  selector: 'app-searchable-select',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './searchable-select.html',
  styleUrls: ['./searchable-select.scss'],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => SearchableSelectComponent),
      multi: true
    }
  ]
})
export class SearchableSelectComponent implements ControlValueAccessor {
  readonly options = input<SelectListItem[]>([]);
  readonly placeholder = input('Select an option');
  readonly emptyLabel = input('No results found');
  readonly isLoading = input(false);

  readonly isOpen = signal(false);
  readonly searchQuery = signal('');
  readonly value = signal<string | null>(null);
  readonly disabled = signal(false);

  readonly selectionChange = output<string | null>();

  private readonly elementRef = inject(ElementRef);

  // Derived state
  readonly selectedOption = computed(() => {
    const val = this.value();
    if (!val) return undefined;
    
    return this.options().find(o => 
      o.id.toString().toLowerCase() === val.toString().toLowerCase()
    );
  });

  readonly filteredOptions = computed(() => {
    const query = this.searchQuery().toLowerCase();
    return this.options().filter(o => o.label.toLowerCase().includes(query));
  });

  // CVA callbacks
  private onChange: (value: string | null) => void = () => {};
  private onTouched: () => void = () => {};

  toggleOpen(): void {
    if (this.disabled() || this.isLoading()) return;
    this.isOpen.update(v => !v);
    if (this.isOpen()) {
      // Create a slight delay to allow rendering before focusing search
      // (Implementation detail found in many dropdowns)
      setTimeout(() => {
        const input = this.elementRef.nativeElement.querySelector('.search-input');
        if (input) input.focus();
      });
    } else {
        this.onTouched();
    }
  }

  close(): void {
    this.isOpen.set(false);
    this.onTouched();
  }

  selectOption(optionId: string | null): void {
    this.value.set(optionId);
    this.onChange(optionId);
    this.selectionChange.emit(optionId);
    this.close();
    this.searchQuery.set('');
  }

  onSearch(event: Event): void {
    const target = event.target as HTMLInputElement;
    this.searchQuery.set(target.value);
  }

  // Close when clicking outside
  @HostListener('document:click', ['$event'])
  onClickOutside(event: Event): void {
    if (!this.elementRef.nativeElement.contains(event.target)) {
      this.isOpen.set(false);
      this.onTouched();
    }
  }

  // CVA Implementation
  writeValue(value: string | null): void {
    this.value.set(value);
  }

  registerOnChange(fn: any): void {
    this.onChange = fn;
  }

  registerOnTouched(fn: any): void {
    this.onTouched = fn;
  }

  setDisabledState(isDisabled: boolean): void {
    this.disabled.set(isDisabled);
  }
}
