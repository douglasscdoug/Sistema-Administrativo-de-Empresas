import { UserRole } from "./user-role";

export interface Usuario {
    id: string;
    nome: string;
    email: string;
    ativo: boolean;
    dataCriacao: Date;
    role: UserRole;
}