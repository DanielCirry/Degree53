using Degree53.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Degree53.ControllersBase
{
    [Authorize]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class Degree53ControllerBase : ControllerBase
    {
        protected string GetClaimValue(string claimType) => HttpContext.User.Claims.Single(c => c.Type == claimType).Value;
        protected Guid CurrentUserId => Guid.Parse(GetClaimValue(ClaimTypes.UserId));

        protected bool IsCurrentUserAdministrator => bool.Parse(GetClaimValue(ClaimTypes.ElevatedRights));
    }
}
