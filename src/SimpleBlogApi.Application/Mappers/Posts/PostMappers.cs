using AutoMapper;
using SimpleBlogApi.Application.Commands.Posts;
using SimpleBlogApi.Application.DTOs.Posts;
using SimpleBlogApi.Application.Results.Posts;
using SimpleBlogApi.Domain.Entities;

namespace SimpleBlogApi.Application.Mappers.Posts;

public static class PostMappers
{
    private static readonly IMapper _mapper = new MapperConfiguration(cfg =>
        cfg.AddProfile<PostProfile>()).CreateMapper();

    public static CreatePostCommand ToCommand(this CreatePostRequestDTO dto)
        => _mapper.Map<CreatePostCommand>(dto);

    public static CreatePostResponseDTO ToPostResponse(this CreatePostResult result)
        => _mapper.Map<CreatePostResponseDTO>(result);

    public static Post ToEntity(this CreatePostCommand command)
        => _mapper.Map<Post>(command);

    public static CreatePostResult ToCreateResult(this Post entity)
        => _mapper.Map<CreatePostResult>(entity);
}