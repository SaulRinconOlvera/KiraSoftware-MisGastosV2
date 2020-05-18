using System;

namespace KiraSolution.Web.Services.Other.Transaction.Exceptions
{
    public class ModelStateException : Exception
    {
        public object ModelState { get; set; }

        public ModelStateException(string message) : base(message) { }
        public ModelStateException(object modelState) => ModelState = modelState;
        public ModelStateException(string message, object modelState) : base(message) => 
            ModelState = modelState;
    }
}
