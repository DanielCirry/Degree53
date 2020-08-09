using Degree53.Authorization;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Degree53.TokensAuthorization
{
    public static class AuthorizationOptionsExtensions
    {
        public static void AddDegree53Policy(this AuthorizationOptions options, Degree53AuthorizationPolicy policy) => options.AddPolicy(policy.Name, policy.Configuration);
    }
}
