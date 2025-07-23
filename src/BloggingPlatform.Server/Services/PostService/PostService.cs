using AutoMapper;
using BloggingPlatform.Data.Entities;
using BloggingPlatform.Data.Repositories;
using BloggingPlatform.Shared.Responses;
using BloggingPlatform.Shared.Responses.Post;

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
    }
}
