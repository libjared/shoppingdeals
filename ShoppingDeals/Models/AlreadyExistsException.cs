using System;

namespace ShoppingDeals.Models
{
    public class AlreadyExistsException : Exception
    {
        public AlreadyExistsException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }

    public class CredentialsException : Exception
    {
    }
}
