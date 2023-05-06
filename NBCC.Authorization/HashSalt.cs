namespace NBCC.Authorization;

public sealed record HashSalt(byte[] Hash, byte[] Password);
