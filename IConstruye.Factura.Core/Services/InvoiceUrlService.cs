using System.Security.Cryptography;
using System.Text;
using IConstruye.Factura.Core.Interfaces;

namespace IConstruye.Factura.Core.Services;

public class InvoiceUrlService : IInvoiceUrlService
{
    public string GenerateInvoiceUrl(string id)
    {
        using var sha = SHA256.Create();
        var hash = sha.ComputeHash(Encoding.UTF8.GetBytes(id));
        var number = BitConverter.ToUInt64(hash, 0);

        const string base62Chars = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
        var sb = new StringBuilder();

        while (number > 0)
        {
            sb.Insert(0, base62Chars[(int)(number % 62)]);
            number /= 62;
        }

        return sb.ToString();
    }
}