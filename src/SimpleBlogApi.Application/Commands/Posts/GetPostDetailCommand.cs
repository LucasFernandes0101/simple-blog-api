using MediatR;
using SimpleBlogApi.Application.Results.Posts;

namespace SimpleBlogApi.Application.Commands.Posts;

public record GetPostDetailCommand(
    int Id) : IRequest<GetPostDetailResult?>;