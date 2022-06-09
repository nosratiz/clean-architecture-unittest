namespace Hastnama.Solico.Common.Helper
{
    public static class PasswordManagement
    {
        
        public static string HashPass(string pass) => BCrypt.Net.BCrypt.HashPassword(pass);


        public static bool CheckPassword(string enterPassword, string password)
            => BCrypt.Net.BCrypt.Verify(enterPassword, password);


    }
}