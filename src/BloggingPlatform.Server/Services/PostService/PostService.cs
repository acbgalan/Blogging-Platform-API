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
    }
}
