using Microsoft.AspNetCore.Authorization;

namespace PawPadIO.Hub.Api.Authorization
{
    public class GlobalPermissionRequirement : IAuthorizationRequirement
    {
        public GlobalPermissionRequirement(string operation)
        {
            Operation = operation;
        }

        public string Operation { get; protected set; }
    }
}
