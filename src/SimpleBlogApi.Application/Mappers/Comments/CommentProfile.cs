using AutoMapper;
using SimpleBlogApi.Application.Commands.Comments;
using SimpleBlogApi.Application.DTOs.Comments;
using SimpleBlogApi.Application.Results.Comments;
using SimpleBlogApi.Domain.Entities;

namespace SimpleBlogApi.Application.Mappers.Comments;

public class CommentProfile : Profile
{
    public CommentProfile()
    {
        CreateMap<Comment, GetCommentResult>().ReverseMap();
        CreateMap<Comment, CreateCommentResult>().ReverseMap();
        CreateMap<Comment, CreateCommentCommand>().ReverseMap();
        CreateMap<GetCommentResult, GetCommentResponseDTO>().ReverseMap();
        CreateMap<CreateCommentResult, CreateCommentResponseDTO>().ReverseMap();
    }
}