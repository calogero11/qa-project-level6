import { getAuthToken } from '../utils/authToken.js'

async function api(url, method = 'GET', body = null) {
    let response;
    
    try {
        const token = getAuthToken()
        
        method.toUpperCase();
        
        let headers = {
            'Content-Type': 'application/json'
        }
        
        if(token) {
            headers.Authorization = `Bearer ${token}` 
        }
        
        if(body)
        {
            body = JSON.stringify(body)
        }
        
        response = await fetch(`${__API_URL__}/${url}`, {
            method: method,
            headers: headers,
            body: body
        });
        
        if (response.ok) {
            const data = await response.text();

            if (!data) {
                return;    
            }

            return JSON.parse(data);
        }
    }
    catch (error) {
        throw new Error("There was an issue with your request.")
    }

    throw new Error(`status: ${response.status}. message: ${response.statusText}`)
}

export default api;