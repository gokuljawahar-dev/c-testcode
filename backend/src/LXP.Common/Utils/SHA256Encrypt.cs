namespace LXP.Common.Utils;

using System.Security.Cryptography;
using System.Text;

public static class SHA256Encrypt
{
    public static string ComputePasswordToSha256Hash(string plainText)
    {
        // Create a SHA256 hash from string
        // Computing Hash - returns here byte array
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(plainText));

        // now convert byte array to a string
        var stringbuilder = new StringBuilder();
        for (var i = 0; i < bytes.Length; i++)
        {
            stringbuilder.Append(bytes[i].ToString("x2"));
        }
        return stringbuilder.ToString();
    }
}
