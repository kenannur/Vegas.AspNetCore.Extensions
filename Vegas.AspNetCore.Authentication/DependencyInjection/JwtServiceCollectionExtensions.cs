using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Vegas.AspNetCore.Authentication.Factory;
using Vegas.AspNetCore.Authentication.Settings;
using Vegas.AspNetCore.Configuration.Extensions;

namespace Vegas.AspNetCore.Authentication.DependencyInjection
{
    public static class JwtServiceCollectionExtensions
    {
        private const string _authenticationScheme = JwtBearerDefaults.AuthenticationScheme;

        public static void AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.ConfigureSettings<IJwtSettings, JwtSettings>(configuration);
            services.AddAuthentication(_authenticationScheme)
                    .AddJwtBearer();
            ConfigureJwt(services);
        }

        public static void AddJwtAuthenticationCore(this IServiceCollection services, IConfiguration configuration)
        {
            services.ConfigureSettings<IJwtSettings, JwtSettings>(configuration);
            services.AddAuthenticationCore(options =>
            {
                options.DefaultAuthenticateScheme = _authenticationScheme;
                options.DefaultChallengeScheme = _authenticationScheme;
                options.DefaultScheme = _authenticationScheme;
            });
            new AuthenticationBuilder(services).AddJwtBearer();
            ConfigureJwt(services);
        }

        private static void ConfigureJwt(IServiceCollection services)
        {
            services.AddScoped<IJwtFactory, JwtFactory>();
            services
                .AddOptions<JwtBearerOptions>(_authenticationScheme)
                .Configure<IJwtSettings>((options, jwtSettings) =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtSettings.Issuer,
                        ValidAudience = jwtSettings.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey))
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnChallenge = context =>
                        {
                            if (context.AuthenticateFailure is SecurityTokenExpiredException)
                            {
                                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                                context.HttpContext.Items.Add("ErrorMessages", new List<string>
                                {
                                                "SecurityTokenExpired_AuthenticationMessage"
                                });
                            }
                            return Task.CompletedTask;
                        },
                        OnForbidden = context =>
                        {
                            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                            context.HttpContext.Items.Add("ErrorMessages", new List<string>
                            {
                                            "Forbidden_AuthenticationMessage"
                            });
                            return Task.CompletedTask;
                        }
                    };
                });
        }
    }
}
