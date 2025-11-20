using AutoMapper;
using SimpleBlogApi.Application.Commands.Posts;
using SimpleBlogApi.Application.DTOs.Posts;
using SimpleBlogApi.Application.Results.Posts;
using SimpleBlogApi.Domain.Entities;

namespace SimpleBlogApi.Application.Mappers.Posts;

public class PostProfile : Profile
{
    public PostProfile()
    {
        CreateMap<Post, CreatePostCommand>().ReverseMap();
        CreateMap<Post, CreatePostResult>().ReverseMap();
        CreateMap<CreatePostResult, CreatePostResponseDTO>().ReverseMap();
        CreateMap<GetPostResponseDTO, GetPostResult>().ReverseMap();
        CreateMap<GetPostCommand, GetPostRequestDTO>().ReverseMap();

        CreateMap<Post, GetPostResult>()
            .ForMember(dest => dest.CommentCount,
                       opt => opt.MapFrom(src => src.Comments.Count));
    }
}