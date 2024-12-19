using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2ID_C06_Verification.utils
{
    public class HttpUtils
    {
        public static string ConvertDictionaryToQueryString(Dictionary<string, string> parameters)
        {
            if (parameters == null || parameters.Count == 0)
                return string.Empty;
            return string.Join("&", parameters
                .Where(kv => !string.IsNullOrWhiteSpace(kv.Key) && kv.Value != null)
                .Select(kv => $"{Uri.EscapeDataString(kv.Key)}={Uri.EscapeDataString(kv.Value)}"));
        }
    }
}
