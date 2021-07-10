using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace PawPadIO.Hub.GraphQL
{
    public class UserContext : Dictionary<string, object>
    {
        [Obsolete]
        public ClaimsPrincipal User { get; set; }

        public DateTimeOffset? SessionValidNotBefore
        {
            get
            {
                return GetSingleValueClaimDateTimeOffset("nbf");
            }
        }

        public DateTimeOffset? SessionIssuedAt
        {
            get
            {
                return GetSingleValueClaimDateTimeOffset("iat");
            }
        }

        public DateTimeOffset SessionValidUntil
        {
            get
            {
                return GetSingleValueClaimDateTimeOffset("exp").Value;
            }
        }

        //TODO: Issuer should probably be a reference to a database entity, but we don't have a table for that yet
        public string Issuer
        {
            get
            {
                return GetSingleValueClaimString("iss");
            }
        }

        public string? Subject
        {
            get
            {
                return GetSingleValueClaimString("sub");
            }
        }

        public string? JwtId
        {
            get
            {
                return GetSingleValueClaimString("jti");
            }
        }

        public IEnumerable<string> Audiences
        {
            get
            {
                return GetMultiValueClaimString("aud");
            }
        }

        public UserContext()
        {

        }

        public UserContext(ClaimsPrincipal principal)
        {
            var claimsTypeGrouped = principal.Claims.GroupBy(c => c.Type);

            foreach (var claimTypeGroup in claimsTypeGrouped)
            {
                switch (claimTypeGroup.Key)
                {
                    // Registered claims we know is a date
                    case "nbf":
                    case "iat":
                    case "exp":
                        var secondsSinceEpoch = Convert.ToInt64(claimTypeGroup.Single().Value);
                        this.Add(claimTypeGroup.Key, DateTimeOffset.FromUnixTimeSeconds(secondsSinceEpoch));
                        break;
                    // Registered claims we know is a single string
                    case "iss":
                    case "sub":
                    case "jti":
                        this.Add(claimTypeGroup.Key, claimTypeGroup.Single().Value);
                        break;
                    // Registered claims we know can be multiple strings, plus everything else
                    case "aud":
                    default:
                        // Select only the Value from each Claim
                        this.Add(claimTypeGroup.Key, claimTypeGroup.Select(c => c.Value));
                        break;
                }
            }

            //{ nbf: 1625883158}
            //{ exp: 1625943158}
            //{ iss: https://127.0.0.1:5001}	
            //{ aud: nz.furry.infursec.hub}
            //{ client_id: insomnia}
            //{ sub: fbbef07e - 2fe0 - 473d - 9f3f - 6eb46a60f769}
            //{ auth_time: 1625883127}
            //{ idp: local}
            //{ given_name: tcfox @tc.nz}
            //{ role: user}
            //{ email: tcfox @tc.nz}
            //{ jti: CC9718802F37619815DE9500F74C4A6C}
            //{ sid: 00A26713C4F806F63526E49168FA3920}
            //{ iat: 1625883158}
            //{ scope: openid}
            //{ scope: profile}
            //{ scope: email}
            //{ scope: hub.read}
            //{ scope: hub.write}
            //{ amr: pwd}
        }

        public bool SessionIsForAudience(string AudienceToCheckFor)
        {
            return Audiences.Contains(AudienceToCheckFor);
        }

        private DateTimeOffset? GetSingleValueClaimDateTimeOffset(string claimType)
        {
            return (DateTimeOffset)this.SingleOrDefault(c => c.Key == claimType).Value;
        }

        private string? GetSingleValueClaimString(string claimType)
        {
            return (string)this.SingleOrDefault(c => c.Key == claimType).Value;
        }

        private IEnumerable<string> GetMultiValueClaimString(string claimType)
        {
            return (IEnumerable<string>)this.Single(c => c.Key == claimType).Value;
        }
    }
}
