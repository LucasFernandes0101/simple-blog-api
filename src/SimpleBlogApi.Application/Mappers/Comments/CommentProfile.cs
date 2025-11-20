using AutoMapper;
using SimpleBlogApi.Application.DTOs.Comments;
using SimpleBlogApi.Application.Results.Comments;
using SimpleBlogApi.Domain.Entities;

namespace SimpleBlogApi.Application.Mappers.Comments;

public class CommentProfile : Profile
{
    public CommentProfile()
    {
        CreateMap<Comment, GetCommentResult>();
        CreateMap<Comment, CreateCommentResult>();
        CreateMap<GetCommentResult, GetCommentResponseDTO>();
    }
}