using AutoMapper;
using SimpleBlogApi.Application.Commands.Posts;
using SimpleBlogApi.Application.DTOs.Comments;
using SimpleBlogApi.Application.DTOs.Posts;
using SimpleBlogApi.Application.Results.Comments;
using SimpleBlogApi.Application.Results.Posts;
using SimpleBlogApi.Domain.Entities;

namespace SimpleBlogApi.Application.Mappers.Posts;

public class BlogPostProfile : Profile
{
    public BlogPostProfile()
    {
        CreateMap<BlogPost, CreatePostCommand>().ReverseMap();
        CreateMap<BlogPost, CreateBlogPostResult>().ReverseMap();
        CreateMap<CreatePostCommand, CreateBlogPostRequestDTO>().ReverseMap();
        CreateMap<CreateBlogPostResult, CreateBlogPostResponseDTO>().ReverseMap();
        CreateMap<GetBlogPostResponseDTO, GetBlogPostResult>().ReverseMap();
        CreateMap<GetPostCommand, GetBlogPostRequestDTO>().ReverseMap();
        CreateMap<BlogPost, GetBlogPostDetailResult>().ReverseMap();
        CreateMap<GetBlogPostDetailResult, GetBlogPostDetailResponseDTO>().ReverseMap();
        CreateMap<GetCommentResult, GetCommentResponseDTO>().ReverseMap();
        CreateMap<Comment, GetCommentResult>().ReverseMap();

        CreateMap<BlogPost, GetBlogPostResult>()
            .ForMember(dest => dest.CommentCount,
                       opt => opt.MapFrom(src => src.Comments.Count()));
    }
}