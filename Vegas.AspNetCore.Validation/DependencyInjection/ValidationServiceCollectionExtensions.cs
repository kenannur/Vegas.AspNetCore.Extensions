using System.Linq;
using System.Net;
using System.Reflection;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Vegas.AspNetCore.Validation.DependencyInjection
{
    public static class ValidationServiceCollectionExtensions
    {
        public static IMvcBuilder AddAutoFluentValidation(this IMvcBuilder mvcBuilder, Assembly assembly)
        {
            return mvcBuilder
                .AddFluentValidation(configuration =>
                {
                    configuration.RegisterValidatorsFromAssembly(assembly);
                    configuration.DisableDataAnnotationsValidation = true;
                })
                .ConfigureApiBehaviorOptions(options =>
                {
                    // Manuel olarak handle etmek için alttaki comment açılmalı ve
                    // Controller method içinde ModelState.IsValid kontrol edilmeli

                    // options.SuppressModelStateInvalidFilter = true;
                    options.InvalidModelStateResponseFactory = context =>
                    {
                        var errorMessages = context.ModelState.Values.SelectMany(v => v.Errors).Select(v => v.ErrorMessage).ToList();

                        context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                        context.HttpContext.Items.Add("ErrorMessages", errorMessages);
                        return new EmptyResult();
                    };
                });
        }
    }
}
