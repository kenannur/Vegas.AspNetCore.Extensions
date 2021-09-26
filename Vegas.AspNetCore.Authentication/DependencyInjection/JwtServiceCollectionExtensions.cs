using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Vegas.AspNetCore.Authentication.Context;
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

            ConfigureJwtAuthentication(services);
        }

        public static void AddJwtAuthentication(this IServiceCollection services, IJwtSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }
            services.AddSingleton(settings);

            ConfigureJwtAuthentication(services);
        }

        private static void ConfigureJwtAuthentication(this IServiceCollection services)
        {
            services.AddScoped<IJwtContext, JwtContext>(sp => new JwtContext(sp.GetRequiredService<IJwtSettings>()));

            services.AddAuthentication(_authenticationScheme)
                    .AddJwtBearer();

            services.AddOptions<JwtBearerOptions>(_authenticationScheme)
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
