using AutoMapper;
using SimpleBlogApi.Application.Commands.BlogPosts;
using SimpleBlogApi.Application.DTOs.Comments;
using SimpleBlogApi.Application.DTOs.BlogPosts;
using SimpleBlogApi.Application.Results.Comments;
using SimpleBlogApi.Application.Results.BlogPosts;
using SimpleBlogApi.Domain.Entities;

namespace SimpleBlogApi.Application.Mappers.BlogPosts;

public class BlogPostProfile : Profile
{
    public BlogPostProfile()
    {
        CreateMap<BlogPost, CreateBlogPostCommand>().ReverseMap();
        CreateMap<BlogPost, CreateBlogPostResult>().ReverseMap();
        CreateMap<CreateBlogPostCommand, CreateBlogPostRequestDTO>().ReverseMap();
        CreateMap<CreateBlogPostResult, CreateBlogPostResponseDTO>().ReverseMap();
        CreateMap<GetBlogPostResponseDTO, GetBlogPostResult>().ReverseMap();
        CreateMap<GetBlogPostCommand, GetBlogPostRequestDTO>().ReverseMap();
        CreateMap<BlogPost, GetBlogPostDetailResult>().ReverseMap();
        CreateMap<GetBlogPostDetailResult, GetBlogPostDetailResponseDTO>().ReverseMap();
        CreateMap<GetCommentResult, GetCommentResponseDTO>().ReverseMap();
        CreateMap<Comment, GetCommentResult>().ReverseMap();

        CreateMap<BlogPost, GetBlogPostResult>()
            .ForMember(dest => dest.CommentCount,
                       opt => opt.MapFrom(src => src.Comments.Count()));
    }
}