using MediatR;
using SimpleBlogApi.Application.Results.Posts;
using SimpleBlogApi.Domain.Base;

namespace SimpleBlogApi.Application.Commands.Posts;

public record GetPostCommand(
    int Page,
    int Size) : IRequest<PagedResult<GetPostResult>>;