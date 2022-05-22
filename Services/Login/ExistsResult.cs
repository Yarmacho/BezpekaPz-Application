namespace Application.Services.Login
{
    public class ExistsResult  : RequestResult
    {
        public ExistsResult()
        {
        }

        public ExistsResult(string message) : base(message)
        {
        }
    }
}
