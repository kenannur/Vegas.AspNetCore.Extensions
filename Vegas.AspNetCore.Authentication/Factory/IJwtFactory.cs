namespace Vegas.AspNetCore.Authentication.Factory
{
    public interface IJwtFactory
    {
        string CreateToken(string forRole);
    }
}
