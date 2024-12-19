using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace _2ID_C06_Verification.utils
{
    public class HashUtils
    {
        public static string HashSHA512(string key, string data)
        {
            try
            {
                if (key == null || data == null)
                    return null;
                byte[] keyBytes = Encoding.UTF8.GetBytes(key);
                using (HMACSHA512 hmac = new HMACSHA512(keyBytes))
                {
                    byte[] dataBytes = Encoding.UTF8.GetBytes(data);
                    byte[] hashBytes = hmac.ComputeHash(dataBytes);
                    StringBuilder sb = new StringBuilder(hashBytes.Length * 2);
                    foreach (byte b in hashBytes)
                    {
                        sb.AppendFormat("{0:x2}", b);
                    }
                    return sb.ToString();
                }
            }
            catch
            {
                return string.Empty;
            }
        }

        public static string BuildHashValue(String apiKey, String transactionId, long timestamp, String message)
        {
            return $"{apiKey}{transactionId}{timestamp}{message}";
        }

        public static string FormatHex(string input)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(input);
            StringBuilder hex = new StringBuilder(bytes.Length * 2);
            foreach (byte b in bytes)
            {
                hex.AppendFormat("{0:x2}", b);
            }
            return hex.ToString();
        }
    }
}
