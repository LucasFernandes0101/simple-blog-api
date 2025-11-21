using AutoMapper;
using SimpleBlogApi.Application.Commands.BlogPosts;
using SimpleBlogApi.Application.DTOs.BlogPosts;
using SimpleBlogApi.Application.Results.BlogPosts;
using SimpleBlogApi.Domain.Entities;

namespace SimpleBlogApi.Application.Mappers.BlogPosts;

public static class BlogPostMappers
{
    private static readonly IMapper _mapper = new MapperConfiguration(_ =>
        _.AddProfile<BlogPostProfile>()).CreateMapper();

    public static CreateBlogPostCommand ToCommand(this CreateBlogPostRequestDTO dto)
        => _mapper.Map<CreateBlogPostCommand>(dto);

    public static GetBlogPostCommand ToCommand(this GetBlogPostRequestDTO dto)
        => _mapper.Map<GetBlogPostCommand>(dto);

    public static List<GetBlogPostResponseDTO> ToDTO(this List<GetBlogPostResult> results)
        => _mapper.Map<List<GetBlogPostResponseDTO>>(results);

    public static List<GetBlogPostResult> ToResult(this List<BlogPost> entites)
        => _mapper.Map<List<GetBlogPostResult>>(entites);

    public static CreateBlogPostResponseDTO ToDTO(this CreateBlogPostResult result)
        => _mapper.Map<CreateBlogPostResponseDTO>(result);

    public static BlogPost ToEntity(this CreateBlogPostCommand command)
        => _mapper.Map<BlogPost>(command);

    public static CreateBlogPostResult ToCreateResult(this BlogPost entity)
        => _mapper.Map<CreateBlogPostResult>(entity);

    public static GetBlogPostDetailResult ToDetailResult(this BlogPost entity)
        => _mapper.Map<GetBlogPostDetailResult>(entity);

    public static GetBlogPostDetailResponseDTO ToDTO(this GetBlogPostDetailResult result)
        => _mapper.Map<GetBlogPostDetailResponseDTO>(result);
}