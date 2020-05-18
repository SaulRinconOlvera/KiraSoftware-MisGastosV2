using System;
using System.Collections.Generic;

namespace KiraSolution.Web.Services.Other.Transaction.Model
{
    public class ErrorAnswer<T> : IErrorAnswer<T> where T : class
    {
        public ErrorAnswer(string message)  {  Message = message; Data = null; }
        public ErrorAnswer(T data) {
            Data = data;
            Message = "Error at processing request.";
        }
        public ErrorAnswer(string message, T data) {
            Data = data;
            Message = message;
        }
        public ErrorAnswer(string message, T data, Exception ex)
        {
            Data = data;
            Message = message;

            ProcessException(ex);
        }
        public ErrorAnswer(Exception ex) { ProcessException(ex); }

        public bool Success => false;

        public string Message { get; private set; }

        public T Data { get; private set; }

        public IList<ErrorViewModel> ErrorList { get; private set; }

        private void ProcessException(Exception ex)
        {
            if (ErrorList is null) ErrorList = new List<ErrorViewModel>();

            ErrorList.Add(new ErrorViewModel()
            {
                Message = ex.Message, Number = ex.HResult, ErrorType = ex.GetType().Name,
                HasInnerException = ex.InnerException != null
            }); ;

            if (ex.InnerException != null) ProcessException(ex.InnerException);
        }
    }
}
