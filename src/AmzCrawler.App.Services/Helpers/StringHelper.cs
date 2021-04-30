using System;

namespace AmzCrawler.App.Services.Helpers
{
    public static class StringHelper
    {
        public static double? ToNullableDouble(this string s)
        {
            if (double.TryParse(s, out double number)) return number;
            return null;
        }

        public static int? ToNullableInt(this string s)
        {
            if (int.TryParse(s, out int number)) return number;
            return null;
        }

        public static bool? ToNullableBoolean(this string s)
        {
            if (s.EqualsIgnoreCase("Yes")) return true;
            if (s.EqualsIgnoreCase("No")) return false;

            if (bool.TryParse(s, out bool result)) return result;
            return null;
        }

        public static bool IsNotNullOrEmpty(this string s)
        {
            return !string.IsNullOrEmpty(s);
        }

        public static bool IsNotNullOrWhiteSpace(this string s)
        {
            return !string.IsNullOrWhiteSpace(s);
        }

        public static bool IsNullOrWhiteSpace(this string s)
        {
            return string.IsNullOrWhiteSpace(s);
        }
        public static bool EqualsIgnoreCase(this string str1, string str2)
        {
            return str1.Equals(str2, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
