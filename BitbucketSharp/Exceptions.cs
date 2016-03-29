using System;
using System.Net;

namespace BitbucketSharp
{
    public class BitbucketException : Exception
    {
        public string Name { get; }
        
        public BitbucketException(string name, string description)
            : base(description)
        {
            Name = name;
        }
    }


    public class ForbiddenException : StatusCodeException
    {
        public ForbiddenException(string title)
            : base(HttpStatusCode.Forbidden, title ?? "You do not have the permissions to access or modify this resource.") { }
    }

    public class NotFoundException : StatusCodeException
    {
        public NotFoundException(string title)
            : base(HttpStatusCode.NotFound, title ?? "The server is unable to locate the requested resource.") { }
    }

    public class InternalServerException : StatusCodeException
    {
        public InternalServerException(string title)
            : base(HttpStatusCode.InternalServerError, title ?? "The request was unable to be processed due to an interal server error.") { }
    }

    public class UnauthoriezdException : StatusCodeException
    {
        public UnauthoriezdException(string title)
            : base(HttpStatusCode.Unauthorized, title ?? "You are not authorized to view this resource.") { }
    }

    public class StatusCodeException : Exception
    {
        public HttpStatusCode StatusCode { get; private set; }

        public StatusCodeException(HttpStatusCode statusCode, string message)
            : base(message ?? statusCode.ToString())
        {
            StatusCode = statusCode;
        }

        public static StatusCodeException FactoryCreate(HttpStatusCode statusCode, string message = null)
        {
            switch (statusCode)
            {
                case HttpStatusCode.Forbidden:
                    return new ForbiddenException(message);
                case HttpStatusCode.Unauthorized:
                    return new UnauthoriezdException(message);
                case HttpStatusCode.NotFound:
                    return new NotFoundException(message);
                case HttpStatusCode.InternalServerError:
                    return new InternalServerException(message);
                default:
                    return new StatusCodeException(statusCode, message);
            }
        }
    }
}

