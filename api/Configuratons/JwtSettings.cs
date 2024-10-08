namespace api.Configuratons
{
    /// <summary>
    /// The JwtSettings class represents the configuration settings for JSON Web Token (JWT) authentication.
    /// </summary>
    /// 
    /// <remarks>
    /// The JwtSettings class is used to store the issuer, audience, access token expiry time, access token secret,
    /// refresh token expiry time, and refresh token secret for JWT authentication.
    /// </remarks>
    public class JwtSettings
    {
        public string Issuer { get; set; } = "";
        public string Audience { get; set; } = "";
        public int AccessExpiryMs { get; set; } = 0;
        public string AccessSecret { get; set; } = "";
        public int RefreshExpiryMs { get; set; } = 0;
        public string RefreshSecret { get; set; } = "";
    }
}