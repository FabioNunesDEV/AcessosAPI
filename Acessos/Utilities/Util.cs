using System.Security.Cryptography;
using System.Text;

namespace Acessos.Utilities
{
    public static class Util
    {
        /// <summary>
        /// Gera um salt aleatório de bytes.
        /// </summary>
        /// <param name="size">O tamanho do salt em bytes. O valor padrão é 16.</param>
        /// <returns>O salt gerado como uma string Base64.</returns>
        public static string GerarSalt(int size = 16)
        {
            byte[] salt = new byte[size];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            return Convert.ToBase64String(salt);
        }

        /// <summary>
        /// Gera um hash SHA256 a partir de uma string.
        /// </summary>
        /// <param name="input">A string de entrada.</param>
        /// <returns>O hash gerado.</returns>
        public static string GerarHash(string input)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = sha256.ComputeHash(inputBytes);
                return Convert.ToBase64String(hashBytes);
            }
        }
    }
}
