using SimpleBlogApi.Domain.Base;

namespace SimpleBlogApi.Domain.Exceptions;

public class PostNotFoundException(string message) : BaseException(message)
{
}