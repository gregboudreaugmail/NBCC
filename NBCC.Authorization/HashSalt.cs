namespace NBCC.Authorization;

public class HashSalt
{
    public byte[] Hash { get; private set; }
    public byte[] Salt { get; private set; }
    public HashSalt(byte[] hash, byte[] salt)
    {
        Hash = hash;
        Salt = salt;
    }
}
