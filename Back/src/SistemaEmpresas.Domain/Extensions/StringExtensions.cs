using System.Text.RegularExpressions;

namespace SistemaEmpresas.Domain.Extensions;

public static class StringExtensions
{
    public static string SomenteNumeros(this string valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
            return string.Empty;

        return Regex.Replace(valor, @"\D", "");
    }
}