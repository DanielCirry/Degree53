using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace Degree53.TokensAuthorization
{
    public class AuthorizeViaJwtBearerTokenAttribute : AuthorizeAttribute
    {
        public AuthorizeViaJwtBearerTokenAttribute(string policy) : base(policy) => AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme;
    }
}
