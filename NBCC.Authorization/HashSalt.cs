namespace NBCC.Authorization;

public class HashSalt
{
    public byte[] Hash { get; private set; }
    public byte[] Password { get; private set; }
    public HashSalt() { }
    public HashSalt(byte[] hash, byte[] password)
    {
        Hash = hash;
        Password = password;
    }
}
