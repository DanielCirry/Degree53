using Degree53.ControllersBase;
using Degree53.Domain.Contracts;
using Degree53.Domain.Models;
using Degree53.TokensAuthorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Degree53.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [EnableCors("MyPolicy")]
    public class Degree53Controller : Degree53ControllerBase
    {
        private readonly IDegree53Service _service;

        public Degree53Controller(IDegree53Service service)
        {
            _service = service;
        }

        [AuthorizeUserViaJwtToken]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("v1/posts")]
        public async Task<IActionResult> GetPostsAsync()
        {
            var posts = await _service.GetPostsAsync();
            if (posts.Count == 0)
                return NotFound();

            return Ok(posts);
        }

        [AuthorizeUserViaJwtToken]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("v1/posts/{postId}")]
        public async Task<IActionResult> GetPostAsync(Guid userId, int postId)
        {
            if (userId != CurrentUserId)
                return Forbid();

            var post = await _service.GetPostAsync(postId);
            if (post == null)
                return NotFound();

            return Ok(post);
        }

        [AuthorizeUserViaJwtToken]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost("v1/post")]
        public async Task<IActionResult> AddPostAsync(Guid userId, [FromBody] PostModel postModel)
        {
            if (userId != CurrentUserId)
                return Forbid();

            if (postModel == null)
                return BadRequest();

            await _service.AddPostAsync(postModel);
            await _service.CompleteTransactionAsync();

            return Ok();
        }
    }
}
