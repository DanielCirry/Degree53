using Degree53.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Degree53.TokensAuthorization
{
    public class AuthorizeAdministratorViaJwtTokenAttribute : AuthorizeViaJwtBearerTokenAttribute
    {
        public AuthorizeAdministratorViaJwtTokenAttribute() : base(Degree53AuthorizationPolicy.ElevatedRights.Name)
        { }
    }
}
