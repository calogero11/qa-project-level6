import api from "./api.js"

export const login = async (email, password) => {
    return await api("auth/login", "POST", { email, password });
}

export const register = async (email, password) => {
    return await api("register", "POST", { email, password });
}