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
            if (string.IsNullOrWhiteSpace(hashAlmacenado)) return false;

            var partes = hashAlmacenado.Split('.', 3);
            if (partes.Length != 3) return false;

            int iteraciones = int.Parse(partes[0]);
            byte[] salt = Convert.FromBase64String(partes[1]);
            byte[] hashEsperado = Convert.FromBase64String(partes[2]);

            byte[] hashCalculado = Rfc2898DeriveBytes.Pbkdf2(
                valorPlano, salt, iteraciones, HashAlgorithmName.SHA256, hashEsperado.Length);

            return CryptographicOperations.FixedTimeEquals(hashCalculado, hashEsperado);
        }
    }
}
