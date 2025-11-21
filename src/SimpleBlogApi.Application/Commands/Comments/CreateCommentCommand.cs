using MediatR;
using SimpleBlogApi.Application.Results.Comments;

namespace SimpleBlogApi.Application.Commands.Comments;

public record CreateCommentCommand(
    int BlogPostId,
    string Content,
    string Author) : IRequest<CreateCommentResult>;