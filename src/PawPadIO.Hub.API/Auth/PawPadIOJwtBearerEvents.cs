using System.Diagnostics;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using PawPadIO.Hub.Domain.Models;
using PawPadIO.Hub.Domain.Services;

namespace PawPadIO.Hub.API.Auth
{
    internal class PawPadIOJwtBearerEvents
    {
        internal static async Task OnTokenValidated(TokenValidatedContext context)
        {
            var userService = context.HttpContext.RequestServices.GetService<IUserService<HubUser>>();

            var identity = (ClaimsIdentity)context.Principal.Identity;
            var user = await userService.GetUserFromIssuerAsync(identity.FindFirst("iss").Value, identity.FindFirst("sub").Value, context.HttpContext.RequestAborted);

            // Create a new user if they do not exist locally fill and in the defaults. 
            if (user == null)
            {
                user = new HubUser
                {
                    Name = identity.Name,
                    Email = identity.FindFirst("email").Value,
                    Issuer = identity.FindFirst("iss").Value,
                    Subject = identity.FindFirst("sub").Value,
                };

                await userService.CreateUserAsync(user);
            }
        }

        internal static Task OnAuthenticationFailed(AuthenticationFailedContext arg)
        {
            Debug.WriteLine("Authentication failed");
            return Task.CompletedTask;
        }

        internal static Task OnChallenge(JwtBearerChallengeContext arg)
        {
            Debug.WriteLine("Not authentication, challenge triggered");
            return Task.CompletedTask;
        }

        internal static Task OnForbidden(ForbiddenContext arg)
        {
            Debug.WriteLine("Access forbidden");
            return Task.CompletedTask;
        }
    }
}
