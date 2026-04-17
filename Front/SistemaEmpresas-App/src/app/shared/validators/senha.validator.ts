import { AbstractControl, ValidationErrors } from "@angular/forms";

export function senhaValidator(control: AbstractControl): ValidationErrors | null {
    const senha = control.value;

    if (!senha) return null;

    const erros: any = {};

    if (senha.length < 8)
        erros.minLength = true;

    if (!/[A-Za-z0]/.test(senha))
        erros.letra = true;

    if (!/\d/.test(senha))
        erros.numero = true;

    return Object.keys(erros).length ? erros : null;
}