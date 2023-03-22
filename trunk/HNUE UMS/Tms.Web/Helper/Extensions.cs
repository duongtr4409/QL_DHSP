using System;
using Hnue.Helper;

namespace Ums.App.Helper
{
    public static class Extensions
    {
        public static DateTime ToAppDate(this string s)
        {
            return s.ToDateTime("dd-MM-yyyy");
        }

        public static string ToAppDate(this DateTime d)
        {
            return d.ToString("dd-MM-yyyy");
        }

        public static string ToAppDate(this DateTime? d)
        {
            return d?.ToString("dd-MM-yyyy") ?? "";
        }

        public static string ToAppDateTime(this DateTime d)
        {
            return d.ToString("dd-MM-yyyy HH:mm");
        }
    }
}