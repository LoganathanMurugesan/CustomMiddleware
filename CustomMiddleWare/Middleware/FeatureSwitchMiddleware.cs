using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomMiddleWare.Middleware
{
    public class FeatureSwitchMiddleware
    {
        private readonly RequestDelegate _next;
        public FeatureSwitchMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext, IConfiguration configuration)
        {
            if (httpContext.Request.Path.Value.Contains("/features"))
            {
                var switches = configuration.GetSection("FeatureSwitches");
                var report = switches.GetChildren().Select(x => $"{x.Key} : {x.Value}");
                await httpContext.Response.WriteAsync(string.Join("\n", report));
            }

            else
            {
                await _next(httpContext);
            }
        }
    }
}
