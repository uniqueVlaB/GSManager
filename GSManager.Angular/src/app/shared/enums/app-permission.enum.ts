/**
 * Known permissions exposed by the API in the JWT `permission` claim.
 * Extend this as the backend adds more granular permissions.
 */
export enum AppPermission {
  FullAccess = 'full_access',
  ReadOnly = 'read_only',
  ManageMembers = 'manage_members',
  ManagePlots = 'manage_plots',
  ManagePayments = 'manage_payments',
  ViewReports = 'view_reports'
}
