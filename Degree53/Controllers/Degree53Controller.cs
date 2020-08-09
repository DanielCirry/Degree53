using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Degree53.ControllersBase;
using Degree53.Domain.Contracts;
using Degree53.Domain.Models;
using Degree53.TokensAuthorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Degree53.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class Degree53Controller : Degree53ControllerBase
    {
        private readonly IDegree53Service _service;

        public Degree53Controller(IDegree53Service service)
        {
            _service = service;
        }

        [AuthorizeUserViaJwtToken]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("v1/post/{postId}")]
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
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
