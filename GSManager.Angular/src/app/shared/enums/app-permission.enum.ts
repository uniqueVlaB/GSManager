/**
 * Known permissions exposed by the API in the JWT `permission` claim.
 * Extend this as the backend adds more granular permissions.
 */
export enum AppPermission {
  FullAccess = 'full_access',

  ViewMembers = 'members:view',
  AddMembers = 'members:add',
  EditMembers = 'members:edit',
  DeleteMembers = 'members:delete',

  ViewPlots = 'plots:view',
  AddPlots = 'plots:add',
  EditPlots = 'plots:edit',
  DeletePlots = 'plots:delete',

  ViewUsers = 'users:view',
  AddUsers = 'users:add',
  EditUsers = 'users:edit',
  DeleteUsers = 'users:delete',
}
