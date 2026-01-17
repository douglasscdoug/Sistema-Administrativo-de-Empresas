using System.Text.RegularExpressions;

namespace SistemaEmpresas.Application.Validators;

public class CnpjValidator
{
    public static bool IsValid(string cnpj)
    {
        if(string.IsNullOrWhiteSpace(cnpj))
            return false;

        // Remove caracteres nao numericos
        cnpj = Regex.Replace(cnpj, @"\D", "");

        if(cnpj.Length != 14)
            return false;

       // Elimina sequencias invalidas 
       if(new string(cnpj[0], cnpj.Length) == cnpj)
            return false;

        int[] multiplicador1 = { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
        int[] multiplicador2 = { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

        string tempCnpj = cnpj[..12];
        int soma = 0;

        for(int i = 0; i < 12; i++)
            soma += (tempCnpj[i] - '0') * multiplicador1[i];

        int resto = soma % 11;
        resto = resto < 2 ? 0 : 11 - resto; 

        string digito = resto.ToString();
        tempCnpj += digito;

        soma = 0;
        for(int i = 0; i < 13; i++)
            soma += (tempCnpj[i] - '0') * multiplicador2[i];

        resto = soma % 11;
        resto = resto < 2 ? 0 : 11 - resto;

        digito += resto.ToString();

        return cnpj.EndsWith(digito); 
    }
}