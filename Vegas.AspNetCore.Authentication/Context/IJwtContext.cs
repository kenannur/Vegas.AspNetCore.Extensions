namespace Vegas.AspNetCore.Authentication.Context
{
    public interface IJwtContext
    {
        string CreateToken(string forRole);
    }
}
