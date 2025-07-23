using AutoMapper;
using BloggingPlatform.Data.Entities;
using BloggingPlatform.Data.Repositories;
using BloggingPlatform.Server.Services.PostService;
using BloggingPlatform.Shared.Requests.Post;
using BloggingPlatform.Shared.Responses.Post;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Data.Common;

namespace BloggingPlatform.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly IPostRepository _postRepository;
        private readonly IMapper _mapper;
        private readonly IPostService _postService;

        public PostsController(IPostRepository postRepository, IMapper mapper, IPostService postService)
        {
            _postRepository = postRepository;
            _mapper = mapper;
            _postService = postService;
        }

        [HttpGet("{id:int}", Name = "GetPost")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PostResponse>> GetPost(int id)
        {
            var response = await _postService.GetPostAsync(id);

            if (!response.Success)
            {
                return NotFound(response.Message);
            }

            return Ok(response.Data);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<PostResponse>>> GetPosts([FromQuery] string? searchTerm)
        {
            var serviceResponse = await _postService.GetPostsAsync(searchTerm);

            if (!serviceResponse.Success)
            {
                return NotFound(serviceResponse.Message);
            }

            return Ok(serviceResponse.Data);
        }


        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> CreatePost([FromBody] CreatePostRequest createPostRequest)
        {
            if (createPostRequest == null)
            {
                return BadRequest();
            }

            var serviceResponse = await _postService.CreatePostAsync(createPostRequest);

            if (!serviceResponse.Success)
            {
                return StatusCode(serviceResponse.StatusCode, serviceResponse.Message);
            }

            return CreatedAtRoute("GetPost", new { id = serviceResponse.Data!.Id }, serviceResponse.Data);
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> UpdatePost(int id, UpdatePostRequest updatePostRequest)
        {
            if(updatePostRequest == null)
            {
                return BadRequest();
            }

            if (id != updatePostRequest.Id)
            {
                return BadRequest("Id mismatch");
            }

            var serviceResponse = await  _postService.UpdatePostAsync(updatePostRequest);

            if (!serviceResponse.Success)
            {
                return StatusCode(serviceResponse.StatusCode, serviceResponse.Message);
            }

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeletePost(int id)
        {
            try
            {
                var exits = await _postRepository.ExitsAsync(id);

                if (!exits)
                {
                    return NotFound("Post not found");
                }

                await _postRepository.DeleteAsync(id);
                int saveResult = await _postRepository.SaveAsync();

                if (!(saveResult > 0))
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Unexpected value when saving");
                }

                return NoContent();
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Database error: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Unexpected error: {ex.Message}");
            }
        }
    }
}
