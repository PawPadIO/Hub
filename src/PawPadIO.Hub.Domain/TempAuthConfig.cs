using System.Collections.Generic;
using IdentityModel;
using IdentityServer4.Models;

namespace PawPadIO.Hub.Domain
{
    // TODO: Put pretty much all of this in the DB instead
    public class TempConfig
    {
        public static IEnumerable<ApiResource> Apis =>
            new List<ApiResource>
            {
                // TODO: This should be a list of allowed scopes
                new ApiResource("PawPadIO.Hub.API", "PawPadIO API")
            };

        public static IEnumerable<Client> Clients =>
            new List<Client>
            {
                new Client
                {
                    ClientId = "devclient",

                    // no interactive user, use the clientid/secret for authentication
                    AllowedGrantTypes = new[] { GrantType.ClientCredentials }, // GrantTypes.ClientCredentials,

                    // secret for authentication
                    ClientSecrets =
                    {
                        new Secret("secret".ToSha256())
                    },

                    // scopes that client has access to
                    AllowedScopes = { "PawPadIO.Hub.API" }
                },
                 new Client
                {
                    ClientId = "devclientuser",

                    // no interactive user, use the clientid/secret for authentication
                    AllowedGrantTypes = new[] { GrantType.AuthorizationCode }, // GrantTypes.ClientCredentials,

                    // secret for authentication
                    ClientSecrets =
                    {
                        new Secret("secret".ToSha256())
                    },

                    RedirectUris = { "https://fake.infursec.furry.nz/got-the-code" },

                    // scopes that client has access to
                    AllowedScopes = { "PawPadIO.Hub.API" }
                },
            };

        //public static List<TestUser> TestUsers =>
        //    new List<TestUser>()
        //        {
        //                    new TestUser
        //                    {
        //                        Username = "admin",
        //                        Password = "testpassword",
        //                    },
        //        };
    }
}
