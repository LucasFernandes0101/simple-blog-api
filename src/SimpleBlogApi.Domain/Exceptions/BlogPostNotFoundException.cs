using SimpleBlogApi.Domain.Base;

namespace SimpleBlogApi.Domain.Exceptions;

public class BlogPostNotFoundException(string message) : BaseException(message)
{
}