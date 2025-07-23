using BloggingPlatform.Data.Entities;
using BloggingPlatform.Shared.Responses;
using BloggingPlatform.Shared.Responses.Post;

namespace BloggingPlatform.Server.Services.PostService
{
    public interface IPostService
    {
        Task<ServiceResult<PostResponse>> GetPostAsync(int id);

    }

}
