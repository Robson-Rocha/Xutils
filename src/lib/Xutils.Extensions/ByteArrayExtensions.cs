namespace Xutils.Extensions
{
    using MimeDetective;
    using System;

    public static class ByteArrayExtensions
    {
        public static string ToDataUrl(this byte[] byteArray, string mimeType) => $"data:{mimeType};base64,{Convert.ToBase64String(byteArray)}";

        public static string ToDataUrl(this byte[] byteArray) => ToDataUrl(byteArray, byteArray.GetFileType()?.Mime ?? "application/octet-stream");
    }
}
