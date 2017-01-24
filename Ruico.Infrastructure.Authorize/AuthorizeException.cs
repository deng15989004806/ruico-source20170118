﻿using System;

namespace Ruico.Infrastructure.Authorize
{
    public class AuthorizeException : Exception
    {
        public AuthorizeException(string message) : base(message)
        {
        }
    }

    public class AuthorizeTokenNotFoundException : Exception
    {
        private const String DefaultMessage = "Authorize Token Not Found";

        public AuthorizeTokenNotFoundException()
            : base(DefaultMessage)
        {
        }
    }

    public class AuthorizeTokenInvalidException : Exception
    {
        private const String DefaultMessage = "Authorize Token Invalid";

        public AuthorizeTokenInvalidException()
            : base(DefaultMessage)
        {
        }
    }

    public class AuthorizeNoPermissionException : Exception
    {
        private const String DefaultMessage = "Authorize No Permission";

        public AuthorizeNoPermissionException()
            : base(DefaultMessage)
        {
        }
    }
}
