using System.Collections.Generic;

namespace Application.Services
{
    public class RequestResult : IRequestResult
    {
        public bool Success { get; }

        public string ErrorMessage { get; }

        private Dictionary<string, object> _parameters = new Dictionary<string, object>();
        public IReadOnlyDictionary<string, object> Parameters => _parameters;

        public RequestResult()
        {
            Success = true;
        }

        public RequestResult(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }

        public RequestResult(string errorFormat, params object[] args) 
            : this(string.Format(errorFormat, args))
        {
        }

        public void AddParameter(string key, object value)
        {
            _parameters[key] = value;
        }
    }
}
