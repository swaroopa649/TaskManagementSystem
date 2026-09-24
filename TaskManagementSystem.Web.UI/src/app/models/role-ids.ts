export const RoleIds = {
  ADMIN: 1,
  MANAGER: 2,
  USER: 3
} as const;

export type RoleId = typeof RoleIds[keyof typeof RoleIds];