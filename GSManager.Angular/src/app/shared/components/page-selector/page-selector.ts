import { ChangeDetectionStrategy, Component, computed, input, output } from "@angular/core";
import { ButtonComponent } from "../button/button";
import { SelectComponent, SelectOption } from "../select/select";
@Component({
  selector: 'app-page-selector',
  imports: [ButtonComponent, SelectComponent],
  templateUrl: './page-selector.html',
  styleUrl: './page-selector.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class PageSelectorComponent {
  readonly currentPage = input(1);
  readonly pageSize = input(10);
  readonly totalPages = input(1);

  readonly pageChange = output<number>();
  readonly pageSizeChange = output<number>();

  readonly pageSizeOptions: SelectOption[] = [
    { value: '10', label: '10' },
    { value: '25', label: '25' },
    { value: '50', label: '50' },
    { value: '100', label: '100' },
  ];

  /**
   * Builds the page number list with null as ellipsis sentinel.
   * E.g. total=10, current=5 → [1, null, 4, 5, 6, null, 10]
   */
  readonly pages = computed((): (number | null)[] => {
    const current = this.currentPage();
    const total = this.totalPages();

    if (total <= 7) {
      return Array.from({ length: total }, (_, i) => i + 1);
    }

    const result: (number | null)[] = [1];

    if (current > 3) result.push(null);

    const start = Math.max(2, current - 1);
    const end = Math.min(total - 1, current + 1);
    for (let i = start; i <= end; i++) {
      result.push(i);
    }

    if (current < total - 2) result.push(null);

    result.push(total);
    return result;
  });

  goToPreviousPage(): void {
    if (this.currentPage() > 1) {
      this.pageChange.emit(this.currentPage() - 1);
    }
  }

  goToNextPage(): void {
    if (this.currentPage() < this.totalPages()) {
      this.pageChange.emit(this.currentPage() + 1);
    }
  }

  goToPage(page: number): void {
    if (page >= 1 && page <= this.totalPages()) {
      this.pageChange.emit(page);
    }
  }

  setPageSize(size: string): void {
    this.pageSizeChange.emit(Number(size));
  }
}