using System;
using System.Text;

namespace MyAudios.MyUiFramework.Utils
{
    public static class ScriptUtils
    {
        public const char STRING_SEPARATOR = '|';
        private const string BASE64_IDENTIFIER = "B64|";
        
        private static readonly bool debug = false;

        public static string DecodeString(string data)
        {
            if (!data.StartsWith(BASE64_IDENTIFIER))
                return data;
            
            byte[] bytes = Convert.FromBase64String(data.Substring(BASE64_IDENTIFIER.Length));
            data = Encoding.UTF8.GetString(bytes);

            return data;
        }

        public static string EncodeString(string data)
        {
            if (debug)
                return data;
            
            return BASE64_IDENTIFIER + Convert.ToBase64String(Encoding.UTF8.GetBytes(data));
        }
    }
}