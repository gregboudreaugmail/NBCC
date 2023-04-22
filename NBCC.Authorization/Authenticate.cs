using System.Security.Cryptography;

namespace NBCC.Authorization;
public sealed class Authenticate
{
    const int HashSize = 512;
    const int SaltSize = 512;
    const int Iterations = 1000;

    public static HashSalt GenerateSaltedHash(string password)
    {
        var saltBytes = new byte[HashSize];
        using (var generator = RandomNumberGenerator.Create())
            generator.GetBytes(saltBytes);

        Rfc2898DeriveBytes pbkdf2 = new(password, saltBytes, Iterations, HashAlgorithmName.SHA512);
        var hashPassword = pbkdf2.GetBytes(SaltSize);

        return new(hashPassword, saltBytes);
    }
    public static bool VerifyPassword(string enteredPassword, byte[] storedHash, byte[] saltBytes)
    {
        var rfc2898DeriveBytes = new Rfc2898DeriveBytes(enteredPassword, saltBytes, Iterations, HashAlgorithmName.SHA512);
        var generatedHash = rfc2898DeriveBytes.GetBytes(SaltSize);
        return generatedHash.SequenceEqual(storedHash);
    }
}