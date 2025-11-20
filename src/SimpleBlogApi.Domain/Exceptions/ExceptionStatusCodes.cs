using System.Net;

namespace SimpleBlogApi.Domain.Exceptions;

public static class ExceptionStatusCodes
{
    private static Dictionary<Type, HttpStatusCode> exceptionsStatusCodes = new()
    {
        {typeof(PostNotFoundException), HttpStatusCode.NotFound},
    };

    public static HttpStatusCode GetExceptionStatusCode(Exception exception)
    {
        var exceptionFound = exceptionsStatusCodes.TryGetValue(
            exception.GetType(),
            out HttpStatusCode statusCode
        );

        return exceptionFound ?
               statusCode :
               HttpStatusCode.InternalServerError;
    }
}
