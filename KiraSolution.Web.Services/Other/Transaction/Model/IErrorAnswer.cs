using System.Collections.Generic;

namespace KiraSolution.Web.Services.Other.Transaction.Model
{
    public interface IErrorAnswer<T> : IAnswerBase<T> where T : class
    {
        IList<ErrorViewModel> ErrorList { get; }
    }
}
