using System;
using System.Security.Cryptography;
using System.Text;

namespace Finall_Project.Helpers
{
    public class PasswordHasher
    {
        public static string HashPass(string password)
        {
            var sha1 = new SHA1CryptoServiceProvider();

            byte[] password_bytes = Encoding.ASCII.GetBytes(password);
            byte[] encrypted_bytes = sha1.ComputeHash(password_bytes);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < encrypted_bytes.Length; i++)
            {
                sb.Append(encrypted_bytes[i].ToString("x2"));
            }
            return sb.ToString();
        }
    }
}
