using System.Net;
using System.Runtime.CompilerServices;

namespace Blocks.Exceptions;

public class HttpException : Exception
{
    public HttpStatusCode HttpStatusCode { get; }
    public HttpException(HttpStatusCode statusCode, string message) : base(string.IsNullOrEmpty(message) ? statusCode.ToString() : message)
    {
        HttpStatusCode = statusCode;
    }
    public HttpException(HttpStatusCode statusCode, string message, Exception ex) : base(string.IsNullOrEmpty(message) ? statusCode.ToString() : message, ex)
    {
        HttpStatusCode = statusCode;
    }
}


