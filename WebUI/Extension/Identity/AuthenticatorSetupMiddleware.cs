using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web
{
    public class AuthenticatorSetupMiddleware
    {
        private readonly RequestDelegate _next;
        private List<string> AllowedControllers = new List<string>
    {
        "/Anonymous",
        "/swagger",
        "/identity/account/login",
    };
        public AuthenticatorSetupMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task Invoke(HttpContext context)
        {
            if (context.User.Identity.IsAuthenticated ||
                context.Request.Method == "OPTIONS" ||
                AllowedControllers.Any(c =>
                {
                    string path = context.Request.Path.ToString().ToLower();
                    return path.StartsWith(c, StringComparison.InvariantCulture);
                }))
            {
                await _next(context);
                return;
            }
            await context.ChallengeAsync("Windows");
        }
    }
}
