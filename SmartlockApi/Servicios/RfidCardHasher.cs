using System.Security.Cryptography;
using System.Text;

namespace SmartlockApi.Servicios;

public sealed class RfidCardHasher(IConfiguration configuration)
{
    public const string PrefijoHash = "RFID:";
    private readonly byte[]? _secreto = ObtenerSecreto(configuration);

    public bool EstaConfigurado => _secreto is { Length: >= 32 };

    public bool TryHashUid(string? uid, out string hash)
    {
        hash = string.Empty;
        if (!EstaConfigurado || !TryNormalizarUid(uid, out string uidNormalizado))
            return false;

        byte[] digest = HMACSHA256.HashData(_secreto!, Encoding.ASCII.GetBytes(uidNormalizado));
        hash = PrefijoHash + Convert.ToHexString(digest);
        return true;
    }

    public static bool EsHashDeTarjeta(string? valor) =>
        valor is { Length: 69 } &&
        valor.StartsWith(PrefijoHash, StringComparison.Ordinal) &&
        valor.AsSpan(PrefijoHash.Length).ToArray().All(Uri.IsHexDigit);

    public static bool TryNormalizarUid(string? uid, out string normalizado)
    {
        normalizado = string.Empty;
        if (string.IsNullOrWhiteSpace(uid))
            return false;

        char[] caracteres = uid
            .Where(caracter => !char.IsWhiteSpace(caracter) && caracter != '-' && caracter != ':')
            .ToArray();
        if (caracteres.Length is not (8 or 14 or 20) || caracteres.Any(caracter => !Uri.IsHexDigit(caracter)))
            return false;

        normalizado = new string(caracteres).ToUpperInvariant();
        return true;
    }

    private static byte[]? ObtenerSecreto(IConfiguration configuration)
    {
        string? secreto = configuration["Rfid:HmacSecret"]
            ?? Environment.GetEnvironmentVariable("SMARTLOCK_RFID_HMAC_SECRET");
        return string.IsNullOrWhiteSpace(secreto) ? null : Encoding.UTF8.GetBytes(secreto);
    }
}
