using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SimpleBlogApi.Application.Commands.Posts;
using SimpleBlogApi.Application.DTOs.Common;
using SimpleBlogApi.Application.DTOs.Posts;
using SimpleBlogApi.Application.Extensions;
using SimpleBlogApi.Application.Mappers.Posts;

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
    [ProducesResponseType(typeof(PagedResponseDTO<GetPostResponseDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult<PagedResponseDTO<GetPostResponseDTO>>> GetAsync(
        [FromQuery] GetPostRequestDTO request)
    {
        var result = await mediator.Send(request.ToCommand());

        if (!result.Items.AnySafe())
            return NoContent();

        return Ok(new PagedResponseDTO<GetPostResponseDTO>(
            result.Items.ToDTO(),
            result.Total,
            request.Page,
            request.Size)
        );
    }

    /// <summary>
    /// Creates a new post.
    /// </summary>
    /// <param name="dto">The post data for creating a new post.</param>
    /// <returns>The created post Id.</returns>
    [HttpPost]
    [ProducesResponseType(typeof(CreatePostResponseDTO), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CreatePostResponseDTO>> PostAsync(
        [FromBody] CreatePostRequestDTO dto)
    {
        var result = await mediator.Send(dto.ToCommand());

        return Created(string.Empty, result.ToDTO());
    }
}