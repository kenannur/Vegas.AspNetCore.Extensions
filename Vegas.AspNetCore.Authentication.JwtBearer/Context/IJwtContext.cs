namespace Vegas.AspNetCore.Authentication.JwtBearer.Context
{
    public interface IJwtContext
    {
        string CreateToken(string forRole);
    }
}
