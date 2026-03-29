import { environment } from "../../../environments/environment";

export const API = {
    base: environment.apiUrl,
    endpoints: {
        auth: `${environment.apiUrl}/auth/login`,
        empresa: `${environment.apiUrl}/empresa`,
        usuario: `${environment.apiUrl}/usuario`
    }
};