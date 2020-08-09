using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Degree53.Authorization
{
    public class Degree53AuthorizationPolicy
    {
        public static Degree53AuthorizationPolicy User = new Degree53AuthorizationPolicy("User", policy => policy.RequireAuthenticatedUser());
        public static Degree53AuthorizationPolicy ElevatedRights = new Degree53AuthorizationPolicy("ElevatedRights", policy => policy.RequireClaim(ClaimTypes.ElevatedRights, true.ToString()));

        public string Name { get; }
        public Action<AuthorizationPolicyBuilder> Configuration { get; }
        public Degree53AuthorizationPolicy(string name, Action<AuthorizationPolicyBuilder> configurePolicy)
        {
            Name = name;
            Configuration = configurePolicy;
        }
    }
}
