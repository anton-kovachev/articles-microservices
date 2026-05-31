using System.Net;

namespace Blocks.Exceptions;

public class NotFoundException : HttpException
{
    public NotFoundException(string message) : base(HttpStatusCode.NotFound, message) {}

    public NotFoundException(string message, Exception ex) : base(HttpStatusCode.NotFound, message, ex) {}
}
