using System.Net;

namespace Blocks.Exceptions;

public class BadRequestException : HttpException
{
    public BadRequestException(string message) : base(HttpStatusCode.BadRequest, message) { }
    public BadRequestException(string message, Exception ex) : base(HttpStatusCode.BadRequest, message, ex) { }
}
