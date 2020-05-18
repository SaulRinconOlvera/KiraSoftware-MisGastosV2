namespace KiraSolution.Web.Services.Other.Transaction.Model
{
    public class SuccessfullyAnswer<T> : ISuccessfullyAnswer<T> where T : class
    {
        public SuccessfullyAnswer() { }
        public SuccessfullyAnswer(T data) { Data = data; Message = "Succesfully executed"; }
        public SuccessfullyAnswer(string mensaje) { Message = mensaje; }
        public SuccessfullyAnswer(string messaje, T data) 
        { Message = messaje; Data = data; }


        public bool Success => true;

        public string Message { get; private set; }

        public T Data { get; private set; }
    }
}
