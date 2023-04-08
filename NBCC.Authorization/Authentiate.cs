using System.Net.WebSockets;
using System.Security.Cryptography;

namespace NBCC.Authorization;
public sealed class Authentiate
{
    const int HASH_SIZE = 512;
    const int SALT_SIZE = 512;
    const int ITERATIONS = 1000;

    public static HashSalt GenerateSaltedHash(string password)
    {
        var saltBytes = new byte[HASH_SIZE];
        using (RandomNumberGenerator generator = RandomNumberGenerator.Create())
            generator.GetBytes(saltBytes);

        Rfc2898DeriveBytes pbkdf2 = new(password, saltBytes, ITERATIONS, HashAlgorithmName.SHA512);
        var hashPassword = pbkdf2.GetBytes(SALT_SIZE);

        return new(hashPassword, saltBytes);
    }
    public static bool VerifyPassword(string enteredPassword, byte[] storedHash, byte[] saltBytes)
    {
        var rfc2898DeriveBytes = new Rfc2898DeriveBytes(enteredPassword, saltBytes, ITERATIONS, HashAlgorithmName.SHA512);
        var generatedHash = rfc2898DeriveBytes.GetBytes(SALT_SIZE);
        return generatedHash.SequenceEqual(storedHash);
    }
}