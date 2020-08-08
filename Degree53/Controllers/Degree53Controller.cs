using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Degree53.Domain.Contracts;
using Degree53.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Degree53.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class Degree53Controller : ControllerBase
    {
        private readonly IDegree53Service _service;

        public Degree53Controller(IDegree53Service service)
        {
            _service = service;
        }

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

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("v1/post/{postId}")]
        public async Task<IActionResult> GetPostAsync( int postId)
        {
            var post = await _service.GetPostAsync(postId);
            if (post == null)
                return NotFound();

            return Ok(post);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPost("v1/post")]
        public async Task<IActionResult> AddPostAsync([FromBody] PostModel postModel)
        {
            if (postModel == null)
                return BadRequest();

            await _service.AddPostAsync(postModel);
            await _service.CompleteTransactionAsync();

            return Ok();
        }
    }
}
