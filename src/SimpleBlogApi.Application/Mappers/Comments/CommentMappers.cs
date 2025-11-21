using AutoMapper;
using SimpleBlogApi.Application.Commands.Comments;
using SimpleBlogApi.Application.DTOs.Comments;
using SimpleBlogApi.Application.Results.Comments;
using SimpleBlogApi.Domain.Entities;

namespace SimpleBlogApi.Application.Mappers.Comments;

public static class CommentMappers
{
    private static readonly IMapper _mapper = new MapperConfiguration(_ =>
        _.AddProfile<CommentProfile>()).CreateMapper();

    public static CreateCommentResult ToCreateResult(this Comment entity)
        => _mapper.Map<CreateCommentResult>(entity);

    public static CreateCommentResponseDTO ToDTO(this CreateCommentResult result)
        => _mapper.Map<CreateCommentResponseDTO>(result);

    public static Comment ToEntity(this CreateCommentCommand command)
        => _mapper.Map<Comment>(command);
}