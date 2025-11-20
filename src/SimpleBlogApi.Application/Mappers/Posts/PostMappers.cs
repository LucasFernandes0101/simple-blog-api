using AutoMapper;
using SimpleBlogApi.Application.Commands.Posts;
using SimpleBlogApi.Application.DTOs.Posts;
using SimpleBlogApi.Application.Results.Posts;
using SimpleBlogApi.Domain.Entities;

namespace SimpleBlogApi.Application.Mappers.Posts;

public static class PostMappers
{
    private static readonly IMapper _mapper = new MapperConfiguration(_ =>
        _.AddProfile<PostProfile>()).CreateMapper();

    public static CreatePostCommand ToCommand(this CreatePostRequestDTO dto)
        => _mapper.Map<CreatePostCommand>(dto);

    public static GetPostCommand ToCommand(this GetPostRequestDTO dto)
        => _mapper.Map<GetPostCommand>(dto);

    public static List<GetPostResponseDTO> ToDTO(this List<GetPostResult> results)
        => _mapper.Map<List<GetPostResponseDTO>>(results);

    public static List<GetPostResult> ToResult(this List<Post> entites)
        => _mapper.Map<List<GetPostResult>>(entites);

    public static CreatePostResponseDTO ToDTO(this CreatePostResult result)
        => _mapper.Map<CreatePostResponseDTO>(result);

    public static Post ToEntity(this CreatePostCommand command)
        => _mapper.Map<Post>(command);

    public static CreatePostResult ToCreateResult(this Post entity)
        => _mapper.Map<CreatePostResult>(entity);

    public static GetPostDetailResult ToDetailResult(this Post entity)
        => _mapper.Map<GetPostDetailResult>(entity);

    public static GetPostDetailResponseDTO ToDTO(this GetPostDetailResult result)
        => _mapper.Map<GetPostDetailResponseDTO>(result);
}