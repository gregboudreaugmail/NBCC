﻿namespace NBCC.Middleware.Extensions;

public static class StringExtensions
{
    public static string? NullIfWhiteSpace(this string s) => string.IsNullOrWhiteSpace(s) ? null : s;
}