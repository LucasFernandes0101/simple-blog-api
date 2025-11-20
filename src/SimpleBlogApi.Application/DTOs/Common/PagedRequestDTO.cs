using Microsoft.AspNetCore.Mvc;

namespace SimpleBlogApi.Application.DTOs.Common;

public record PagedRequestDTO
{
    [FromQuery(Name = "_page")]
    public int Page { get; set; } = 1;

    [FromQuery(Name = "_size")]
    public int Size { get; set; } = 10;
}