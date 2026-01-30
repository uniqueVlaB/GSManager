import { Component, input, output } from "@angular/core";
import { ButtonComponent } from "../button/button";

@Component({
  selector: 'app-page-selector',
  standalone: true,
  imports: [ButtonComponent],
  templateUrl: './page-selector.html',
  styleUrls: ['./page-selector.scss'],
})
export class PageSelectorComponent {
    readonly currentPage = input(1);
    readonly pageSize = input(10);
    readonly totalPages = input(1);

    readonly pageChange = output<number>();
    readonly pageSizeChange = output<number>();


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

    setPageSize(size: number | string): void {
        this.pageSizeChange.emit(Number(size));
    }
 }