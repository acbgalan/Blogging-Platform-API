using BloggingPlatform.Data.Entities;
using BloggingPlatform.Shared.Requests.Post;
using BloggingPlatform.Shared.Responses;
using BloggingPlatform.Shared.Responses.Post;

namespace BloggingPlatform.Server.Services.PostService
{
    public interface IPostService
    {
        Task<ServiceResult<PostResponse>> GetPostAsync(int id);
        Task<ServiceResult<List<PostResponse>>> GetPostsAsync(string? searchTerm);
        Task<ServiceResult<PostResponse>> CreatePostAsync(CreatePostRequest createPostRequest);
        Task<ServiceResult<bool?>> UpdatePostAsync(UpdatePostRequest updatePostRequest);
        Task<ServiceResult<bool?>> DeletePostAsync(int id);
    }

}
