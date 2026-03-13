namespace LXP.Common.Utils;

using System.Security.Cryptography;
using System.Text;

public class Encryption
{
    public static string ComputePasswordToSha256Hash(string plainText)
    {
        // Create a SHA256 hash from string
        // Computing Hash - returns here byte array
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(plainText));

        // now convert byte array to a string
        var stringbuilder = new StringBuilder();
        foreach (var b in bytes)
        {
            stringbuilder.Append(b.ToString("x2"));
        }
        return stringbuilder.ToString();
    }
}
