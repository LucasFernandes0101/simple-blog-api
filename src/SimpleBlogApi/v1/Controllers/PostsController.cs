using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SimpleBlogApi.Application.Commands.Posts;
using SimpleBlogApi.Application.DTOs.Common;
using SimpleBlogApi.Application.DTOs.Posts;
using SimpleBlogApi.Application.Mappers.Posts;

namespace SimpleBlogApi.v1.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class PostsController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Creates a new post.
    /// </summary>
    /// <param name="dto">The post data for creating a new post.</param>
    /// <returns>The created post Id.</returns>
    [HttpPost]
    [ProducesResponseType(typeof(CreatePostCommand), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CreatePostResponseDTO>> PostAsync(
        [FromBody] CreatePostRequestDTO dto)
    {
        var command = dto.ToCommand();

        var result = await mediator.Send(command);

        var response = result.ToPostResponse();

        return Created(string.Empty, response);
    }
}