using System.Text;
using System.IO;
using System.Security.Cryptography;

namespace SharpUpdate
{
    /// <summary>
    /// The allowed HashType algorithms
    /// </summary>
    internal enum HashType
    {
        MD5,
        SHA1,
        SHA512
    }
    
    /// <summary>
    /// The class used for hashing operations, done when doing program updates.
    /// </summary>
    internal static class Hasher
    {
        /// <summary>
        /// Returns the appropriate hash string, based off a file and algorithm used.
        /// </summary>
        /// <param name="filePath">The location where the file to hash is located.</param>
        /// <param name="algo">The HashType algorithm (MD5, SHA1, or SHA512).</param>
        /// <returns>A hashed string.</returns>
        internal static string HashFile(string filePath, HashType algo)
        {
            switch (algo)
            {
                case HashType.MD5:
                    return MakeHashString(MD5.Create().ComputeHash(new FileStream(filePath, FileMode.Open)));
                case HashType.SHA1:
                    return MakeHashString(SHA1.Create().ComputeHash(new FileStream(filePath, FileMode.Open)));
                case HashType.SHA512:
                    return MakeHashString(SHA512.Create().ComputeHash(new FileStream(filePath, FileMode.Open)));
                default:
                    return "";
            }
        }

        /// <summary>
        /// Calculates the string of the hashed sequence.
        /// </summary>
        /// <param name="hash">The calculated sequence of bytes, to convert to string.</param>
        /// <returns>A string representation of the hash.</returns>
        private static string MakeHashString(byte[] hash)
        {
            StringBuilder s = new StringBuilder(hash.Length * 2);
            foreach (byte b in hash)
            {
                s.Append(b.ToString("X2").ToUpper());
            }
            return s.ToString();
        }
    }
}
