using Degree53.Authorization;

namespace Degree53.TokensAuthorization
{
    public class AuthorizeAdministratorViaJwtTokenAttribute : AuthorizeViaJwtBearerTokenAttribute
    {
        public AuthorizeAdministratorViaJwtTokenAttribute() : base(Degree53AuthorizationPolicy.ElevatedRights.Name)
        { }
    }
}
