﻿using System;

namespace RootHub.Contract.Exceptions
{
    public class RootHubAlreadyStartedException : Exception
    {
        public RootHubAlreadyStartedException()
        {
        }

        public RootHubAlreadyStartedException(string message)
            : base(message)
        {
        }

        public RootHubAlreadyStartedException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
