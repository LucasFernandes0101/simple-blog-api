using SimpleBlogApi.Domain.Base;

namespace SimpleBlogApi.Domain.Entities;

public class Post : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;

    public virtual List<Comment> Comments { get; set; } = [];
}