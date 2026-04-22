using System;
using System.Text;

namespace CSAT
{
    public static class Base64Helper
    {
        /// Encode string (UTF-8) -> Base64
        public static string Encode(string plainText)
        {
            if (string.IsNullOrEmpty(plainText))
                return plainText;

            var bytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(bytes);
        }

        /// Decode Base64 -> string (UTF-8)
        public static string Decode(string base64)
        {
            if (string.IsNullOrEmpty(base64))
                return base64;

            try
            {
                var bytes = Convert.FromBase64String(base64);
                return Encoding.UTF8.GetString(bytes);
            }
            catch
            {
                // không phải base64 → trả nguyên
                return base64;
            }
        }

    }
}
