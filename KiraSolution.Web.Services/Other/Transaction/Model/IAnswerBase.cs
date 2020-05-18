namespace KiraSolution.Web.Services.Other.Transaction.Model
{
    public interface IAnswerBase<T> where T : class
    {
        bool Success { get; }
        string Message { get; }
        T Data { get; }
    }
}
