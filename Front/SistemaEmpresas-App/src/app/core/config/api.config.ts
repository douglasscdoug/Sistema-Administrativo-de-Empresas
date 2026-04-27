import { environment } from "../../../environments/environment";

export const API = {
    base: environment.apiUrl,
    endpoints: {
        auth: {
            login: `${environment.apiUrl}/auth/login`,
            refresh: `${environment.apiUrl}/auth/refresh`,
            logout: `${environment.apiUrl}/auth/logout`
        },
        empresa: `${environment.apiUrl}/empresa`,
        usuario: `${environment.apiUrl}/usuario`
    }
};