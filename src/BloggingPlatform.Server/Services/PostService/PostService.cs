using AutoMapper;
using BloggingPlatform.Data.Entities;
using BloggingPlatform.Data.Repositories;
using BloggingPlatform.Shared.Requests.Post;
using BloggingPlatform.Shared.Responses;
using BloggingPlatform.Shared.Responses.Post;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace BloggingPlatform.Server.Services.PostService
{
    public class PostService : IPostService
    {
        private readonly IPostRepository _postRepository;
        private readonly IMapper _mapper;

        public PostService(IPostRepository postRepository, IMapper mapper)
        {
            _postRepository = postRepository;
            _mapper = mapper;
        }

        public async Task<ServiceResult<PostResponse>> GetPostAsync(int id)
        {
            var post = await _postRepository.GetAsync(id);
            var postResponse = _mapper.Map<PostResponse>(post);

            var serviceResult = new ServiceResult<PostResponse>()
            {
                Data = postResponse,
                Success = postResponse != null,
                Message = postResponse != null ? "Post retrieved" : "Post not found",
                StatusCode = postResponse != null ? StatusCodes.Status200OK : StatusCodes.Status404NotFound
            };

            return serviceResult;
        }

        public async Task<ServiceResult<List<PostResponse>>> GetPostsAsync(string? searchTerm)
        {
            var posts = new List<Post>();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                posts = await _postRepository.SearchPostsAsync(searchTerm);
            }
            else
            {
                posts = await _postRepository.GetAllAsync();
            }

            var postsReponse = _mapper.Map<List<PostResponse>>(posts);

            var serviceResult = new ServiceResult<List<PostResponse>>
            {
                Data = postsReponse,
                Success = postsReponse.Any(),
                Message = postsReponse.Any() ? "Posts retrieved" : "Posts not found",
                StatusCode = postsReponse.Any() ? StatusCodes.Status200OK : StatusCodes.Status404NotFound
            };

            return serviceResult;
        }

        public async Task<ServiceResult<PostResponse>> CreatePostAsync(CreatePostRequest createPostRequest)
        {
            var serviceResult = new ServiceResult<PostResponse>();

            try
            {
                var post = _mapper.Map<Post>(createPostRequest);
                await _postRepository.AddAsync(post);
                int saveResult = await _postRepository.SaveAsync();
                var postResponse = _mapper.Map<PostResponse>(post);

                serviceResult = new ServiceResult<PostResponse>
                {
                    Data = postResponse,
                    Success = saveResult > 0,
                    Message = saveResult > 0 ? "Post created successfully" : "Unexpected value when saving",
                    StatusCode = saveResult > 0 ? StatusCodes.Status201Created : StatusCodes.Status500InternalServerError
                };
            }
            catch (DbUpdateException ex)
            {
                serviceResult = new ServiceResult<PostResponse>
                {
                    Data = null,
                    Success = false,
                    Message = $"Database error: {ex.Message}",
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }
            catch (Exception ex)
            {
                serviceResult = new ServiceResult<PostResponse>
                {
                    Data = null,
                    Success = false,
                    Message = $"Unexpected error: {ex.Message}",
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }

            return serviceResult;
        }

        public async Task<ServiceResult<bool?>> UpdatePostAsync(UpdatePostRequest updatePostRequest)
        {
            var serviceResult = new ServiceResult<bool?>();

            try
            {
                var postDb = await _postRepository.GetAsync(updatePostRequest.Id);

                if (postDb == null)
                {
                    serviceResult.Data = null;
                    serviceResult.Success = false;
                    serviceResult.Message = "Post not found";
                    serviceResult.StatusCode = StatusCodes.Status404NotFound;

                    return serviceResult;
                }

                postDb = _mapper.Map(updatePostRequest, postDb);

                await _postRepository.UpdateAsync(postDb);
                int saveResult = await _postRepository.SaveAsync();

                if (!(saveResult > 0))
                {
                    serviceResult.Data = null;
                    serviceResult.Success = false;
                    serviceResult.Message = "Unexpected value when saving";
                    serviceResult.StatusCode = StatusCodes.Status500InternalServerError;
                }

                serviceResult.Data = null;
                serviceResult.Success = true;
                serviceResult.Message = "Post updated successfully";
                serviceResult.StatusCode = StatusCodes.Status204NoContent;
            }
            catch (DbUpdateException ex)
            {
                serviceResult.Data = null;
                serviceResult.Success = false;
                serviceResult.Message = $"Database error: {ex.Message}";
                serviceResult.StatusCode = StatusCodes.Status500InternalServerError;
            }
            catch (Exception ex)
            {
                serviceResult.Data = null;
                serviceResult.Success = false;
                serviceResult.Message = $"Unexpected error: {ex.Message}";
                serviceResult.StatusCode = StatusCodes.Status500InternalServerError;
            }

            return serviceResult;
        }
    }
}
