using System.Net;

namespace ServicoMLBEntidades.Application.Common.Exceptions;

public abstract class AppException : Exception
{
    protected AppException(string message, HttpStatusCode statusCode) : base(message)
    {
        StatusCode = statusCode;
    }

    public HttpStatusCode StatusCode { get; }
}

public class NotFoundException : AppException
{
    public NotFoundException(string message) : base(message, HttpStatusCode.NotFound)
    {
    }
}

public class ConflictException : AppException
{
    public ConflictException(string message) : base(message, HttpStatusCode.Conflict)
    {
    }
}

public class UnauthorizedException : AppException
{
    public UnauthorizedException(string message) : base(message, HttpStatusCode.Unauthorized)
    {
    }
}

public class ForbiddenException : AppException
{
    public ForbiddenException(string message) : base(message, HttpStatusCode.Forbidden)
    {
    }
}

public class BusinessRuleException : AppException
{
    public BusinessRuleException(string message) : base(message, HttpStatusCode.BadRequest)
    {
    }
}
