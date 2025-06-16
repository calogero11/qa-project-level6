import api from "./api.js"

export const createFeed = async (title, content) => {
    return await api("feed", "POST", { title, content });
}

export const getFeeds = async () => {
    return await api("feed");
}

export const updateFeed = async (id, title, content) => {
    return await api(`feed/${id}`, "PUT", { title, content });
}

export const deleteFeed = async (id) => {
    return await api(`feed/${id}`, "DELETE");
}