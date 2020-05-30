using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

using PawPadIO.Hub.Domain.Models;
using PawPadIO.Hub.Domain.Services;

namespace PawPadIO.Hub.API.Auth
{
    public static class NzFursOpenIdConnectEvents
    {
        public static async Task OnUserInformationReceived(UserInformationReceivedContext context)
        {
            var userService = context.HttpContext.RequestServices.GetService<IUserService>();
            var linkGenerator = context.HttpContext.RequestServices.GetService<LinkGenerator>();
            var identity = (ClaimsIdentity)context.Principal.Identity;
            var user = await userService.GetUserFromIssuerAsync(context.Principal.FindFirst("iss").Value, context.Principal.FindFirst("sub").Value, context.HttpContext.RequestAborted);

            // Create a new user if they do not exist locally fill and in the defaults. 
            if (user == null)
            {
                var contextUser = context.User.RootElement;
                user = new User
                {
                    Name = contextUser.GetProperty("nickname").GetString(),
                    Email = contextUser.GetProperty("email").GetString(),
                };
                user.LinkedAccounts = new List<LinkedAccount>
                {
                    new LinkedAccount
                    {
                        Issuer = context.Principal.FindFirst("iss").Value,
                        Subject = context.Principal.FindFirst("sub").Value,
                        User = user,
                    }
                };

                await userService.CreateUserAsync(user);

                // Give new uers a chance to update their details
                // 1. Save redirect URL to session
                    // context.HttpContext.Session.Set(AccountController.RedirectAfterDetailsSessionKey, context.Properties.RedirectUri);
                // 2. Show firtst time page asking for name, details etc,
                    // context.Properties.RedirectUri = linkGenerator.GetUriByAction(context.HttpContext, nameof(AccountController.Index), "Account");
                // 3. if skipped or accepted, redirect to initial redirect page (See AccountController.UpdateAccount)
            }

            identity.AddClaim(new Claim("user", user.Id.ToString()));
            //identity.AddClaim(new Claim("admin", user.IsAdmin.ToString()));
        }
    }
}
