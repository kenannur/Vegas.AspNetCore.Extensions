using System;

namespace Vegas.AspNetCore.Authentication.Context
{
    [Obsolete("Use IJwtFactory instead of this")]
    public interface IJwtContext
    {
        string CreateToken(string forRole);
    }
}
