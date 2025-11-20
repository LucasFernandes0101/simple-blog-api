using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace SimpleBlogApi.v1.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class PostsController(IMediator mediator) : ControllerBase
{
}