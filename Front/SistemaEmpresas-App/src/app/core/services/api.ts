import { environment } from "../../../environments/environment";

export const API = {
    base: environment.apiUrl,
    empresa: `${environment.apiUrl}/empresa`,
    usuario: `${environment.apiUrl}/usuario`,
    auth: `${environment.apiUrl}/auth`
};