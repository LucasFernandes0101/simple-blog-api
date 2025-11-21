using AutoMapper;
using SimpleBlogApi.Application.Commands.Posts;
using SimpleBlogApi.Application.DTOs.Posts;
using SimpleBlogApi.Application.Results.Posts;
using SimpleBlogApi.Domain.Entities;

namespace SimpleBlogApi.Application.Mappers.Posts;

public static class BlogPostMappers
{
    private static readonly IMapper _mapper = new MapperConfiguration(_ =>
        _.AddProfile<BlogPostProfile>()).CreateMapper();

    public static CreatePostCommand ToCommand(this CreateBlogPostRequestDTO dto)
        => _mapper.Map<CreatePostCommand>(dto);

    public static GetPostCommand ToCommand(this GetBlogPostRequestDTO dto)
        => _mapper.Map<GetPostCommand>(dto);

    public static List<GetBlogPostResponseDTO> ToDTO(this List<GetBlogPostResult> results)
        => _mapper.Map<List<GetBlogPostResponseDTO>>(results);

    public static List<GetBlogPostResult> ToResult(this List<BlogPost> entites)
        => _mapper.Map<List<GetBlogPostResult>>(entites);

    public static CreateBlogPostResponseDTO ToDTO(this CreateBlogPostResult result)
        => _mapper.Map<CreateBlogPostResponseDTO>(result);

    public static BlogPost ToEntity(this CreatePostCommand command)
        => _mapper.Map<BlogPost>(command);

    public static CreateBlogPostResult ToCreateResult(this BlogPost entity)
        => _mapper.Map<CreateBlogPostResult>(entity);

    public static GetBlogPostDetailResult ToDetailResult(this BlogPost entity)
        => _mapper.Map<GetBlogPostDetailResult>(entity);

    public static GetBlogPostDetailResponseDTO ToDTO(this GetBlogPostDetailResult result)
        => _mapper.Map<GetBlogPostDetailResponseDTO>(result);
}