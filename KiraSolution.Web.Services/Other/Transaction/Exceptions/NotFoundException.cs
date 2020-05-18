using System;

namespace KiraSolution.Web.Services.Other.Transaction.Exceptions
{
    public class NotFoundException : Exception 
    {
        public NotFoundException() { }
        public NotFoundException(string message) : base(message) { }
    } 
}
