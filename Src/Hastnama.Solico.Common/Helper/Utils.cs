using System;
using System.Text;

namespace Hastnama.Solico.Common.Helper
{
    public static class Utils
    {
        public static string SetHashBasicAuth(string username, string Password)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(username + ":" + Password));
        }
    }
}