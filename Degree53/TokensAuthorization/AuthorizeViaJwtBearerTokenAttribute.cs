using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Degree53.TokensAuthorization
{
    public class AuthorizeViaJwtBearerTokenAttribute : AuthorizeAttribute
    {
        public AuthorizeViaJwtBearerTokenAttribute(string policy) : base(policy) => AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme;
    }
}
