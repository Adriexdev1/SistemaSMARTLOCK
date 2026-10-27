using System.Security.Cryptography;

namespace SmartlockApi.Servicios
{
    public static class PasswordHasher
    {
        private const int TamanoSalt = 16;      // 128 bits
        private const int TamanoHash = 32;      // 256 bits
        private const int Iteraciones = 100_000;

        public static string Hash(string valorPlano)
        {
            byte[] salt = RandomNumberGenerator.GetBytes(TamanoSalt);
            byte[] hash = Rfc2898DeriveBytes.Pbkdf2(
                valorPlano, salt, Iteraciones, HashAlgorithmName.SHA256, TamanoHash);

            return $"{Iteraciones}.{Convert.ToBase64String(salt)}.{Convert.ToBase64String(hash)}";
        }
        public static bool Verificar(string valorPlano, string hashAlmacenado)
        {
            if (!TryObtenerComponentes(hashAlmacenado, out int iteraciones, out byte[] salt, out byte[] hashEsperado))
                return false;

            byte[] hashCalculado = Rfc2898DeriveBytes.Pbkdf2(
                valorPlano, salt, iteraciones, HashAlgorithmName.SHA256, hashEsperado.Length);

            return CryptographicOperations.FixedTimeEquals(hashCalculado, hashEsperado);
        }

        public static bool EsHashAlmacenado(string? valor)
        {
            return TryObtenerComponentes(valor, out _, out _, out _);
        }

        private static bool TryObtenerComponentes(
            string? valor,
            out int iteraciones,
            out byte[] salt,
            out byte[] hash)
        {
            iteraciones = 0;
            salt = Array.Empty<byte>();
            hash = Array.Empty<byte>();

            if (string.IsNullOrWhiteSpace(valor))
                return false;

            string[] partes = valor.Split('.', 3);
            if (partes.Length != 3 ||
                !int.TryParse(partes[0], out iteraciones) ||
                iteraciones <= 0 || iteraciones > 1_000_000)
            {
                return false;
            }

            try
            {
                salt = Convert.FromBase64String(partes[1]);
                hash = Convert.FromBase64String(partes[2]);
            }
            catch (FormatException)
            {
                return false;
            }

            return salt.Length == TamanoSalt && hash.Length == TamanoHash;
        }
    }
}
