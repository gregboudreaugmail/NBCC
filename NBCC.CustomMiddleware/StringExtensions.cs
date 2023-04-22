namespace NBCC.WebApplicaion.Extentions;

public static class StringExtensions
{
    public static string? NullIfEmpty(this string s) => string.IsNullOrEmpty(s) ? null : s;
    public static string? NullIfWhiteSpace(this string s) => string.IsNullOrWhiteSpace(s) ? null : s;
}