﻿namespace NBCC.Authorization.Models;

public sealed record HashSalt(byte[] Hash, byte[] Password);
