import { HttpClient } from "@angular/common/http";
import { computed, inject, Injectable, signal } from "@angular/core";
import { ToastService } from "./toast.service";
import { environment } from "../../../environments/environment";
import { PaginatedResponse, RoleDto, RoleQueryParams, SelectListItem } from "../../shared/models";
import { firstValueFrom } from "rxjs";
import { HttpUtils } from "../utils";

@Injectable({
    providedIn: 'root'
})
export class RoleService {
    private readonly http = inject(HttpClient);
    private readonly toastService = inject(ToastService);
    private readonly apiUrl = `${environment.apiUrl}/roles`;

    // State signals
    private readonly pagedRolesSignal = signal<PaginatedResponse<RoleDto> | null>(null);
    private readonly roleSelectListSignal = signal<SelectListItem[]>([]);
    private readonly loadingSignal = signal(false);

    // Public readonly signals
    readonly pagedRoles = this.pagedRolesSignal.asReadonly();
    readonly roles = computed(() => this.pagedRolesSignal()?.items || []);
    readonly roleSelectList = this.roleSelectListSignal.asReadonly();
    readonly loading = this.loadingSignal.asReadonly();

    async getRoles(params?: RoleQueryParams): Promise<void> {
        const httpParams = HttpUtils.createParams(params);

        this.loadingSignal.set(true);
        try {
            const roles = await firstValueFrom(this.http.get<PaginatedResponse<RoleDto>>(this.apiUrl, { params: httpParams }));
            this.pagedRolesSignal.set(roles);
        } catch (err) {
            console.error('Failed to load roles:', err);
            this.toastService.error('Failed to load roles');
        } finally {
            this.loadingSignal.set(false);
        }
    }

    async getSelectList(): Promise<void> {
        this.loadingSignal.set(true);
        try {
            const selectList = await firstValueFrom(this.http.get<SelectListItem[]>(`${this.apiUrl}/select-list`));
            this.roleSelectListSignal.set(selectList);
        } catch (err) {
            console.error('Failed to load role select list:', err);
            this.toastService.error('Failed to load role options');
        } finally {
            this.loadingSignal.set(false);
        }
    }
}