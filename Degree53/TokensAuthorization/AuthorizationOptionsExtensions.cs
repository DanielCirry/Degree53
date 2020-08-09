using Degree53.Authorization;
using Microsoft.AspNetCore.Authorization;

namespace Degree53.TokensAuthorization
{
    public static class AuthorizationOptionsExtensions
    {
        public static void AddDegree53Policy(this AuthorizationOptions options, Degree53AuthorizationPolicy policy) => options.AddPolicy(policy.Name, policy.Configuration);
    }
}
