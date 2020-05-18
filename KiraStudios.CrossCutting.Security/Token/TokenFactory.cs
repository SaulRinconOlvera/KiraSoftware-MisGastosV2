using System;
using System.Security.Cryptography;

namespace KiraStudios.CrossCutting.Security.Token
{
    public static class TokenFactory
    {
        public static string GenerateToken(int size = 32)
        {
            var randomNumber = new byte[size];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }
    }
}
