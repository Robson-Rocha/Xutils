namespace Xutils.Extensions
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;
    using System.Text.RegularExpressions;

    public static class StringExtensions
    {
        public static string UnAccent(this string text)
        {
            return string.Concat(text.Normalize(NormalizationForm.FormD)
                                     .Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark))
                         .Normalize(NormalizationForm.FormC);
        }

        public static string Slugify(this string text)
        {
            string slug = text.UnAccent().ToLowerInvariant();
            slug = Regex.Replace(slug, @"[^a-z0-9\s-]", "");
            slug = Regex.Replace(slug, @"[\s+]", " ");
            slug = Regex.Replace(slug, @"[\s]", "-");
            return slug;
        }

        public static string GenerateRandomString(ushort length)
        {
            var chars = "abcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars = new char[length];
            var random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            return new string(stringChars);
        }

        public static string ToMD5Hash(this string sourceString)
        {
            var sourceBytes = Encoding.ASCII.GetBytes(sourceString);
            var hashBytes = new MD5CryptoServiceProvider().ComputeHash(sourceBytes);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
                sb.Append(hashBytes[i].ToString("x2"));
            return sb.ToString();
        }

        public static string WhenNullOrWhitespace(this string text, Func<string> valueFunc)
            => string.IsNullOrWhiteSpace(text) ? valueFunc.Invoke() : text;

    }
}
