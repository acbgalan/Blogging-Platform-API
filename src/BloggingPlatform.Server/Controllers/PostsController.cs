using BloggingPlatform.Data.Entities;
using BloggingPlatform.Data.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace BloggingPlatform.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly IPostRepository _postRepository;

        public PostsController(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }

        [HttpGet("{id:int}", Name = "GetPost")]
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
        public async Task<ActionResult<List<Post>>> GetPosts()
        {
            var posts = await _postRepository.GetAllAsync();

            if (posts.Count == 0)
            {
                return NotFound("Posts not found");
            }

            return Ok(posts);
        }

        [HttpPost]
        public async Task<ActionResult> CreatePost([FromBody] Post post)
        {
            if (post == null)
            {
                return BadRequest();
            }

            await _postRepository.AddAsync(post);
            int saveResult = await _postRepository.SaveAsync();

            if (!(saveResult > 0))
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Unexpected value when saving");
            }

            return CreatedAtRoute("GetPost", new { id = post.Id }, post);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> UpdatePost(int id, Post post)
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

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeletePost(int id)
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

    }
}
