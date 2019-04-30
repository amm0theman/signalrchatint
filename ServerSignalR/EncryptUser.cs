using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BCrypt.Net;

namespace ServerSignalR
{
    public class EncryptUser
    {
        static string salt;

        public static string GetRandomSalt()
        {
            return BCrypt.Net.BCrypt.GenerateSalt(12);
        }

        public static string HashUsernamePassword(string password)
        {
            salt = GetRandomSalt();
            return BCrypt.Net.BCrypt.HashPassword(password, salt);
        }

        public static bool ValidateUsernamePassword(string password, string correctHash)
        {
            return BCrypt.Net.BCrypt.Verify(password, correctHash);
        }
    }
}
