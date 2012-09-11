using System;
using System.Net;

namespace BitbucketSharp
{
    public class ForbiddenException : StatusCodeException
    {
        public ForbiddenException()
            : base(HttpStatusCode.Forbidden, "You do not have the permissions to access or modify this resource.")
        {
        }
    }

    public class StatusCodeException : Exception
    {
        public HttpStatusCode StatusCode { get; private set; }

        public StatusCodeException(HttpStatusCode statusCode)
            : this(statusCode, "The resource requested is " + statusCode)
        {
        }

        public StatusCodeException(HttpStatusCode statusCode, string message)
            : base(message)
        {
            StatusCode = statusCode;
        }

        public static StatusCodeException FactoryCreate(HttpStatusCode statusCode)
        {
            if (statusCode == HttpStatusCode.Forbidden)
                return new ForbiddenException();
            else
                return new StatusCodeException(statusCode);
        }
    }
}

