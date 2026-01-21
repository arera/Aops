using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AOps.Application.Common.Utility
{
    public static class PasswordGenerator
    {
        private const string Upper = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private const string Lower = "abcdefghijklmnopqrstuvwxyz";
        private const string Digits = "0123456789";
        private const string Specials = "!@#$%^&*()-_=+<>?";

        public static string GenerateRandomPassword(int length = 12)
        {
            if (length < 8)
                throw new ArgumentException("Password length should be at least 8 characters.");

            string allChars = Upper + Lower + Digits + Specials;
            char[] password = new char[length];
            using (var rng = RandomNumberGenerator.Create())
            {
                byte[] randomBytes = new byte[length];
                rng.GetBytes(randomBytes);

                for (int i = 0; i < length; i++)
                {
                    password[i] = allChars[randomBytes[i] % allChars.Length];
                }
            }

            // Ensure password has at least one of each type
            if (!password.Any(c => Upper.Contains(c))) password[0] = Upper[new Random().Next(Upper.Length)];
            if (!password.Any(c => Lower.Contains(c))) password[1] = Lower[new Random().Next(Lower.Length)];
            if (!password.Any(c => Digits.Contains(c))) password[2] = Digits[new Random().Next(Digits.Length)];
            if (!password.Any(c => Specials.Contains(c))) password[3] = Specials[new Random().Next(Specials.Length)];

            // Shuffle
            return new string(password.OrderBy(_ => Guid.NewGuid()).ToArray());
        }
    }
}
