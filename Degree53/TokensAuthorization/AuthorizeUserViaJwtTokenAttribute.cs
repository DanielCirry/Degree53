using Degree53.Authorization;

namespace Degree53.TokensAuthorization
{
    public class AuthorizeUserViaJwtTokenAttribute : AuthorizeViaJwtBearerTokenAttribute
    {
        public AuthorizeUserViaJwtTokenAttribute() : base(Degree53AuthorizationPolicy.User.Name)
        { }
    }
}
