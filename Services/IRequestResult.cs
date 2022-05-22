using System.Collections.Generic;

namespace Application.Services
{
    public interface IRequestResult
    {
        bool Success { get; }

        string ErrorMessage { get; }

        IReadOnlyDictionary<string, object> Parameters { get; }
    }
}
