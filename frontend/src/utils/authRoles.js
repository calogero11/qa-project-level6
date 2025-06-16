const ROLES_KEY = 'roles'

export const getAuthRoles = () => sessionStorage.getItem(ROLES_KEY);

export const setAuthRoles = (roles) => sessionStorage.setItem(ROLES_KEY, roles);

export const clearAuthRoles = () => sessionStorage.removeItem(ROLES_KEY);