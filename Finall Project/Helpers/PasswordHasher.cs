using System;
using System.Security.Cryptography;
using System.Text;

namespace Finall_Project.Helpers
{
    public class PasswordHasher
    {
        public static string hashPass(string password)
        {
            var sha1 = new SHA1CryptoServiceProvider();

            byte[] password_bytes = Encoding.ASCII.GetBytes(password);
            byte[] encrypted_bytes = sha1.ComputeHash(password_bytes);
            return Convert.ToBase64String(encrypted_bytes);
        }
    }
}
