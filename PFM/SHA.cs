using System.Security.Cryptography;
using System.Text;

namespace PFM
{
    /// <summary>
    /// Class for creating hash for the password
    /// </summary>
    public static class SHA
    {
        /// <summary>
        /// Generates the hash of the given string
        /// </summary>
        /// <param name="inputString">String which needs to be converted to hash</param>
        /// <returns></returns>
        public static string GenerateSHA256String(string inputString)
        {
            SHA256 sha256 = SHA256Managed.Create();
            byte[] bytes = Encoding.UTF8.GetBytes(inputString);
            byte[] hash = sha256.ComputeHash(bytes);
            return GetStringFromHash(hash);
        }

        /// <summary>
        /// Gives back the string from a byte hash
        /// </summary>
        /// <param name="hash">Hash in byte format</param>
        /// <returns></returns>
        private static string GetStringFromHash(byte[] hash)
        {
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                result.Append(hash[i].ToString("X2"));
            }
            return result.ToString();
        }
    }
}
