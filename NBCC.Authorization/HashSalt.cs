namespace NBCC.Authorization;

public class HashSalt
{
    public byte[] Hash { get; } = new byte[32];
    public byte[] Password { get; } = new byte[32];
    public HashSalt() { }
    public HashSalt(byte[] hash, byte[] password)
    {
        Hash = hash;
        Password = password;
    }
}
