const TOKEN_KEY = 'token'

export const getAuthToken = () => sessionStorage.getItem(TOKEN_KEY);

export const setAuthToken = (token) => sessionStorage.setItem(TOKEN_KEY, token);

export const clearAuthToken = () => sessionStorage.removeItem(TOKEN_KEY);