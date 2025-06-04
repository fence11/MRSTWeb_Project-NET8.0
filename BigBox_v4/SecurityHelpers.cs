using System.Security.Cryptography;
using System.Text;

namespace BigBox_v4
{
    public static class SecurityHelpers
    {
        public static string Hash(string input)
        {
            using var sha = SHA256.Create();
            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(input));
            return Convert.ToHexString(bytes);
        }
    }
}
