namespace Vegas.AspNetCore.Authentication.JwtBearer.Settings
{
    public class JwtSettings : IJwtSettings
    {
        public string SecretKey { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string ExpireMinutesFromNow { get; set; }

        public int GetExpireMinutesValue() =>
            int.TryParse(ExpireMinutesFromNow, out int result) ? result : 30;

    }

    public interface IJwtSettings
    {
        string SecretKey { get; set; }
        string Issuer { get; set; }
        string Audience { get; set; }
        string ExpireMinutesFromNow { get; set; }

        int GetExpireMinutesValue();
    }
}
