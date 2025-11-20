using AutoMapper;
using SimpleBlogApi.Application.Commands.Posts;
using SimpleBlogApi.Application.Results.Posts;
using SimpleBlogApi.Domain.Entities;

namespace SimpleBlogApi.Application.Mappers.Posts;

public class PostProfile : Profile
{
    public PostProfile()
    {
        CreateMap<Post, CreatePostCommand>().ReverseMap();
        CreateMap<Post, CreatePostResult>().ReverseMap();
    }
}