using AutoMapper;
using SimpleBlogApi.Application.Commands.Posts;
using SimpleBlogApi.Application.DTOs.Comments;
using SimpleBlogApi.Application.DTOs.Posts;
using SimpleBlogApi.Application.Results.Comments;
using SimpleBlogApi.Application.Results.Posts;
using SimpleBlogApi.Domain.Entities;

namespace SimpleBlogApi.Application.Mappers.Posts;

public class PostProfile : Profile
{
    public PostProfile()
    {
        CreateMap<Post, CreatePostCommand>().ReverseMap();
        CreateMap<Post, CreatePostResult>().ReverseMap();
        CreateMap<CreatePostCommand, CreatePostRequestDTO>().ReverseMap();
        CreateMap<CreatePostResult, CreatePostResponseDTO>().ReverseMap();
        CreateMap<GetPostResponseDTO, GetPostResult>().ReverseMap();
        CreateMap<GetPostCommand, GetPostRequestDTO>().ReverseMap();
        CreateMap<Post, GetPostDetailResult>().ReverseMap();
        CreateMap<GetPostDetailResult, GetPostDetailResponseDTO>().ReverseMap();
        CreateMap<GetCommentResult, GetCommentResponseDTO>().ReverseMap();
        CreateMap<Comment, GetCommentResult>().ReverseMap();

        CreateMap<Post, GetPostResult>()
            .ForMember(dest => dest.CommentCount,
                       opt => opt.MapFrom(src => src.Comments.Count()));
    }
}