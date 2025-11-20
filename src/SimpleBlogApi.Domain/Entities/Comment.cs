using SimpleBlogApi.Domain.Base;

namespace SimpleBlogApi.Domain.Entities;

public class Comment : BaseEntity
{
    public string Content { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public int PostId { get; set; }

    public virtual Post Post { get; set; } = default!;
}