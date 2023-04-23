using Microsoft.Extensions.Configuration;

namespace Organiza.Domain.Config
{
    public struct Database
    {
        public string ConnectinString { get; set; }
    }
    public struct Token
    {
        public string SecretKey { get; set; }
        public string Audience { get; set; }
        public string Issuer { get; set; }
        public byte TokenHoursToExpire { get; set; }
        public byte RefreshTokenDaysToExpire { get; set; }
        public string VerificationCodeHashSalt { get; set; }
    }

    public static class Settings
    {
        public static Database Database { get; private set; }
        public static Token Token { get; private set; }

        public static void Configure(IConfigurationRoot configuration)
        {
            Database = new Database
            {
                ConnectinString = configuration.GetSection("ConnectionStrings:Context").Value
            };

            Token = new Token
            {
                SecretKey = configuration.GetSection("JwtBearerTokenSettings:SecretKey").Value,
                Audience = configuration.GetSection("JwtBearerTokenSettings:Audience").Value,
                Issuer = configuration.GetSection("JwtBearerTokenSettings:Issuer").Value,
                TokenHoursToExpire = (byte)int.Parse(configuration.GetSection("JwtBearerTokenSettings:TokenHoursToExpire").Value),
                RefreshTokenDaysToExpire = (byte)int.Parse(configuration.GetSection("JwtBearerTokenSettings:RefreshTokenDaysToExpire").Value),
                VerificationCodeHashSalt = configuration.GetSection("JwtBearerTokenSettings:VerificationCodeHashSalt").Value,
            };
        }
    }
}
