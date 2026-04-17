import { AbstractControl, ValidationErrors } from "@angular/forms";

export function cnpjValidator(control: AbstractControl): ValidationErrors | null {
    let cnpj = control.value;

    if (!cnpj) return null;

    //Remove tudo que não for número
    cnpj = cnpj.replace(/\D/g, '');

    //Deve ter 14 digitos
    if (cnpj.length !== 14) {
        return { cnpjInvalido: true };
    }

    //Elimina sequências inválidas
    if (/^(\d)\1+$/.test(cnpj)) return { cnpjInvalido: true };

    //Validação do primeiro dígito
    let tamanho = 12;
    let numeros = cnpj.substring(0, tamanho);
    let digitos = cnpj.substring(tamanho);

    let soma = 0;
    let pos = tamanho - 7;

    for (let i = tamanho; i >= 1; i--) {
        soma += parseInt(numeros.charAt(tamanho - i)) * pos--;
        if (pos < 2) pos = 9;
    }

    let resultado = soma % 11 < 2 ? 0 : 11 - (soma % 11);

    if (resultado !== parseInt(digitos.charAt(0))) {
        return { cnpjInvalido: true };
    }

    //Validação do segundo dígito
    tamanho = 13;
    numeros = cnpj.substring(0, tamanho);
    soma = 0;
    pos = tamanho - 7;

    for (let i = tamanho; i >= 1; i--){
        soma += parseInt(numeros.charAt(tamanho - i)) * pos--;
        if (pos < 2) pos = 9;
    }

    resultado = soma % 11 < 2 ? 0 : 11 - (soma % 11);
    if (resultado !== parseInt(digitos.charAt(1))) {
        return { cnpjInvalido: true };
    }

    return null;
}