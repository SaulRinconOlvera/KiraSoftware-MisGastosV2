using KiraSudios.Application.IdentityViewModel.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace KiraSolution.Web.Services.Other.Configuration
{
    public static class AppConfiguration
    {
        public const string GeneralAccessPolicyName = "GeneralAccess";
        public static string ImagesPath { get; set; }
        public static bool Application_ManageActionValidations { get; set; }
        public static bool Application_ManageSpecialActionValidations { get; set; }
        public static double TokenExpiration { get; set; }
        public static string TokenSecretKey { get; set; }
        public static string TokenAlgorithm { get; set; }
        public static string TokenAudiencie { get; set; }
        public static string TokenIssuer { get; set; }
        public static bool Token_ValidateIssuer { get; set; }
        public static bool Token_ValidateAudience { get; set; }
        public static bool Token_ValidateLifetime { get; set; }
        public static bool Token_ValidateIssuerSigningKey { get; set; }
        public static IEnumerable<RoleControlViewModel> RoleControls { get; set; }

        public static void Initialize_AppSettings(IConfiguration configuration) 
        {
            Token_ValidateIssuer =
                Convert.ToBoolean(configuration["JwtBearer:TokenValidationParameters:ValidateIssuer"]);
            Token_ValidateAudience =
                Convert.ToBoolean(configuration["JwtBearer:TokenValidationParameters:ValidateAudience"]);
            Token_ValidateLifetime =
                Convert.ToBoolean(configuration["JwtBearer:TokenValidationParameters:ValidateLifetime"]);
            Token_ValidateIssuerSigningKey =
                Convert.ToBoolean(configuration["JwtBearer:TokenValidationParameters:ValidateIssuerSigningKey"]);
            TokenSecretKey = configuration["JwtBearer:SecretKey"];
            TokenExpiration = Convert.ToDouble(configuration["JwtBearer:Expiration"]);
            TokenAlgorithm = configuration["JwtBearer:Algorithm"];
            TokenAudiencie = configuration["JwtBearer:Audience"];
            TokenIssuer = configuration["JwtBearer:Issuer"];
            ImagesPath = configuration["Application:GeneralConfiguration:ImagesPath"];
        }
    }
}
