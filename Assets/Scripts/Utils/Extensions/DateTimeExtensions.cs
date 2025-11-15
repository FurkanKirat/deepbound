using System;

namespace Utils.Extensions
{
    public static class DateTimeExtensions
    {
        public static string ToFileTimeString(this DateTime dateTime)
            => dateTime.ToString("yyyy-MM-dd_HH-mm-ss");
        
        public static string ToLogString(this DateTime dateTime)
            => dateTime.ToString("yyyy-MM-dd HH:mm:ss");
    }
}