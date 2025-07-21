using AutoMapper;
using BloggingPlatform.Data.Entities;
using BloggingPlatform.Data.Repositories;
using BloggingPlatform.Shared.Requests.Post;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace BloggingPlatform.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly IPostRepository _postRepository;
        private readonly IMapper _mapper;

        public PostsController(IPostRepository postRepository, IMapper mapper)
        {
            _postRepository = postRepository;
            _mapper = mapper;
        }

        [HttpGet("{id:int}", Name = "GetPost")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Post>> GetPost(int id)
        {
            var post = await _postRepository.GetAsync(id);

            if (post == null)
            {
                return NotFound("Post not found");
            }

            return Ok(post);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<Post>>> GetPosts([FromQuery] string? searchTerm)
        {
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var posts = await _postRepository.SearchPostsAsync(searchTerm);

                if (posts.Count == 0)
                {
                    return NotFound("Posts not found");
                }

                return Ok(posts);
            }
            else
            {
                var posts = await _postRepository.GetAllAsync();

                if (posts.Count == 0)
                {
                    return NotFound("Posts not found");
                }

                return Ok(posts);
            }
        }


        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> CreatePost([FromBody] CreatePostRequest createPostRequest)
        {
            try
            {
                if (createPostRequest == null)
                {
                    return BadRequest();
                }

                var post = _mapper.Map<Post>(createPostRequest);


                await _postRepository.AddAsync(post);
                int saveResult = await _postRepository.SaveAsync();

                if (!(saveResult > 0))
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Unexpected value when saving");
                }

                return CreatedAtRoute("GetPost", new { id = post.Id }, post);
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

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> UpdatePost(int id, Post post)
        {
            try
            {
                if (post == null || id != post.Id)
                {
                    return BadRequest("Id mismatch");
                }

                var postDb = await _postRepository.GetAsync(id);

                if (postDb == null)
                {
                    return NotFound("Post not found");
                }

                postDb.Title = post.Title;
                postDb.Content = post.Content;
                postDb.Category = post.Category;
                postDb.Tags = post.Tags;

                await _postRepository.UpdateAsync(postDb);
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
