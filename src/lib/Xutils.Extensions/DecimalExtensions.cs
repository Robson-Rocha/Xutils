namespace Xutils.Extensions
{
    using System;

    public static class DecimalExtensions
    {
        private readonly static string[] byteSizeUnit = { "B", "KB", "MB", "GB", "TB", "PB", "EB" };

        public static string ToByteSize(this decimal byteCount)
        {
            return ToByteSize((long)byteCount);
        }

        public static string ToByteSize(this long byteCount)
        {
            if (byteCount == 0)
                return $"0 {byteSizeUnit[0]}";
            long bytes = Math.Abs(byteCount);
            int place = Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)));
            double num = Math.Round(bytes / Math.Pow(1024, place), 1);
            return $"{Math.Sign(byteCount) * num} {byteSizeUnit[place]}";
        }
    }
}
