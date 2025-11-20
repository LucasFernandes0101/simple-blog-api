using MediatR;
using SimpleBlogApi.Application.Results.Comments;

namespace SimpleBlogApi.Application.Commands.Comments;

public record CreateCommentCommand(
    int PostId,
    string Content,
    string Author) : IRequest<CreateCommentResult>;