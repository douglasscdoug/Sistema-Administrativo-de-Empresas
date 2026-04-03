import { Contato } from "./contato";
import { Endereco } from "./endereco";

export interface Empresa {
    id: string;
    razaoSocial: string;
    cnpj: string;
    ativo: boolean;
    
    endereco: Endereco;
    contato: Contato;
}