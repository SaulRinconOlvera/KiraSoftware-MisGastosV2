using KiraSolution.Web.Services.Other.Configuration;
using KiraStudios.Application.TokenViewModel.Tracking;
using KiraSudios.Application.IdentityViewModel.Identity;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace KiraSolution.Web.Services.Other.JWT
{
    public static class Token
    {

        public static TrackingTokenViewModel Build(UserViewModel user)
        {
            var trackingToken = new TrackingTokenViewModel();
            var current = DateTime.UtcNow;
            var expiration = current.AddMinutes(AppConfiguration.TokenExpiration);

            List<Claim> claims = GetClaims(user, current, expiration, trackingToken);

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(GetTokenDescriptor(claims, expiration));
            trackingToken.Token = tokenHandler.WriteToken(token);
            trackingToken.Claims = claims;
            return trackingToken;
        }

        public static string RefreshToken(UserViewModel user)
        {
            var current = DateTime.UtcNow;
            var expiration = current.AddMinutes(AppConfiguration.TokenExpiration);

            List<Claim> claims = GetClaims(user, current, expiration, new TrackingTokenViewModel());

            var key = Encoding.UTF8.GetBytes(AppConfiguration.TokenSecretKey);
            var creds = new SigningCredentials(
                    new SymmetricSecurityKey(key), AppConfiguration.TokenAlgorithm);

            var token = new JwtSecurityToken(
                issuer: AppConfiguration.TokenIssuer,
                audience: AppConfiguration.TokenAudiencie,
                claims: claims,
                expires: expiration,
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        internal static string GetTokenId(string currentJWToken)
        {
            if (string.IsNullOrWhiteSpace(currentJWToken)) return string.Empty;

            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var token = tokenHandler.ReadJwtToken(currentJWToken);
                return token.Id;
            }
            catch { return string.Empty; }
        }

        internal static bool ValidateToken(string currentJWToken, TrackingTokenViewModel trackingToken, string currentUser)
        {
            bool resultado = false;
            if (!AreParametesOk(currentJWToken, trackingToken, currentUser)) return resultado;
            if (!trackingToken.UserName.Equals(currentUser)) return resultado;

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.ReadJwtToken(currentJWToken);

            if (!trackingToken.Issuer.Equals(token.Issuer)) return resultado;
            if (!CompareDates(trackingToken.Exp, token.ValidTo)) return resultado;

            var iatDate = GetDateValue(
                token.Claims.Where(c => c.Type == JwtRegisteredClaimNames.Iat).FirstOrDefault().Value);
            var nbfDate = GetDateValue(
                token.Claims.Where(c => c.Type == JwtRegisteredClaimNames.Nbf).FirstOrDefault().Value);
            var aud = token.Claims.Where(c => c.Type == JwtRegisteredClaimNames.Aud).FirstOrDefault().Value;
            var tokenAlg = token.SignatureAlgorithm;

            if (!CompareDates(trackingToken.Iat, iatDate)) return resultado;
            if (!CompareDates(trackingToken.Nbf, nbfDate)) return resultado;
            if (!trackingToken.Audience.Equals(aud)) return resultado;
            if (!AppConfiguration.TokenAlgorithm.Equals(tokenAlg)) return resultado;

            return true;
        }

        private static DateTime GetDateValue(string value) =>
            EpochTime.DateTime(Convert.ToInt64(value)).ToUniversalTime();

        private static bool AreParametesOk(string currentJWToken, TrackingTokenViewModel trackingToken, string currentUser)
        {
            return !string.IsNullOrWhiteSpace(currentJWToken) &&
                !string.IsNullOrWhiteSpace(currentUser) &&
                trackingToken != null;
        }

        private static bool CompareDates(DateTime date1, DateTime date2)
        {
            var xdate1 = new DateTime(
                    date1.Year, date1.Month, date1.Day,
                    date1.Hour, date1.Minute, date1.Second);

            var xdate2 = new DateTime(
                date2.Year, date2.Month, date2.Day,
                date2.Hour, date2.Minute, date2.Second);

            return xdate1.CompareTo(xdate2) == 0;
        }

        private static SecurityTokenDescriptor GetTokenDescriptor(List<Claim> claims, DateTime expiration)
        {
            var key = Encoding.UTF8.GetBytes(AppConfiguration.TokenSecretKey);

            return new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = expiration,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key), AppConfiguration.TokenAlgorithm)
            };
        }

        private static List<Claim> GetClaims(
            UserViewModel user, DateTime current, DateTime expiration, TrackingTokenViewModel trackingToken)
        {
            trackingToken.Audience = AppConfiguration.TokenAudiencie;
            trackingToken.Exp = expiration;
            trackingToken.Iat = current;
            trackingToken.Issuer = AppConfiguration.TokenIssuer;
            trackingToken.Nbf = current;
            trackingToken.TokenId = Guid.NewGuid();
            trackingToken.UserEmail = user.Email;
            trackingToken.UserName = user.UserName;
            trackingToken.UserId = user.Id;

            var currentString = new DateTimeOffset(current).ToUnixTimeSeconds().ToString();
            var otroString = EpochTime.GetIntDate(current).ToString();

            try
            {
                var claims = new List<Claim>()
                {
                    new Claim(JwtRegisteredClaimNames.Jti, trackingToken.TokenId.ToString()),
                    new Claim(ClaimTypes.NameIdentifier , trackingToken.UserName),
                    new Claim(ClaimTypes.Name , trackingToken.UserName),
                    new Claim(JwtRegisteredClaimNames.Email , trackingToken.UserEmail),
                    new Claim(JwtRegisteredClaimNames.NameId, user.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat,currentString, ClaimValueTypes.String),
                    new Claim(JwtRegisteredClaimNames.Nbf, otroString, ClaimValueTypes.String),
                    new Claim(JwtRegisteredClaimNames.Exp, EpochTime.GetIntDate(trackingToken.Exp).ToString()),
                    new Claim(JwtRegisteredClaimNames.Aud, trackingToken.Audience),
                    new Claim(JwtRegisteredClaimNames.Iss, trackingToken.Issuer),
                    new Claim(ClaimTypes.Role, user.RolesNames, ClaimValueTypes.String, trackingToken.Issuer)
                };

                return claims;
            }
            catch (Exception ex)
            {
                string a = ex.Message;
                return null;
            }
        }
    }
}
