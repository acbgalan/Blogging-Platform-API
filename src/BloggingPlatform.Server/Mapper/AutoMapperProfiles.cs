using AutoMapper;
using BloggingPlatform.Data.Entities;
using BloggingPlatform.Shared.Requests.Post;
using BloggingPlatform.Shared.Responses.Post;

namespace BloggingPlatform.Server.Mapper
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<CreatePostRequest, Post>()
                .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.Tags.Select(tagName => new Tag { Name = tagName }).ToList()));

            CreateMap<Post, PostResponse>()
                .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.Tags.Select(tagName => tagName.Name).ToList()));


        }


    }
}
