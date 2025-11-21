using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SimpleBlogApi.Application.Commands.Comments;
using SimpleBlogApi.Application.Commands.BlogPosts;
using SimpleBlogApi.Application.DTOs.Comments;
using SimpleBlogApi.Application.DTOs.Common;
using SimpleBlogApi.Application.DTOs.BlogPosts;
using SimpleBlogApi.Application.Extensions;
using SimpleBlogApi.Application.Mappers.Comments;
using SimpleBlogApi.Application.Mappers.BlogPosts;

namespace SimpleBlogApi.v1.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class PostsController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Retrieves a paged list of posts based on the provided filters.
    /// </summary>
    /// <param name="request">The request containing filters for retrieving posts.</param>
    /// <returns>A paged response containing a list of posts.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(PagedResponseDTO<GetBlogPostResponseDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult<PagedResponseDTO<GetBlogPostResponseDTO>>> GetAsync(
        [FromQuery] GetBlogPostRequestDTO request)
    {
        var result = await mediator.Send(request.ToCommand());

        if (!result.Items.AnySafe())
            return NoContent();

        return Ok(new PagedResponseDTO<GetBlogPostResponseDTO>(
            result.Items.ToDTO(),
            result.Total,
            request.Page,
            request.Size)
        );
    }

    /// <summary>
    /// Retrieves detailed information about a post by its ID, including its comments.
    /// </summary>
    /// <param name="id">The post identifier.</param>
    /// <returns>The detailed information of the post, including its comments.</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(GetBlogPostDetailResponseDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<GetBlogPostDetailResponseDTO>> GetAsync(
        [FromRoute] int id)
    {
        var command = new GetBlogPostDetailCommand(id);

        var post = await mediator.Send(command);

        if (post is null)
            return NotFound();

        return Ok(post.ToDTO());
    }

    /// <summary>
    /// Creates a new post.
    /// </summary>
    /// <param name="dto">The post data for creating a new post.</param>
    /// <returns>The created post Id.</returns>
    [HttpPost]
    [ProducesResponseType(typeof(CreateBlogPostResponseDTO), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CreateBlogPostResponseDTO>> PostAsync(
        [FromBody] CreateBlogPostRequestDTO dto)
    {
        var result = await mediator.Send(dto.ToCommand());

        return Created(string.Empty, result.ToDTO());
    }

    /// <summary>
    /// Creates a new comment on a post.
    /// </summary>
    /// <param name="dto">The comment data for creating a new comment.</param>
    [HttpPost("{id}/comments")]
    [ProducesResponseType(typeof(CreateBlogPostResponseDTO), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CreateBlogPostResponseDTO>> PostCommentAsync(
        [FromRoute] int id,
        [FromBody] CreateCommentRequestDTO dto)
    {
        var command = new CreateCommentCommand(
            id,
            dto.Content,
            dto.Author);

        var result = await mediator.Send(command);

        return Created(string.Empty, result.ToDTO());
    }
}