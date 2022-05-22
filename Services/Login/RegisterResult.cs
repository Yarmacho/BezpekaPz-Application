namespace Application.Services.Login
{
    public class RegisterResult : RequestResult
    {
        public RegisterResult()
        {
        }

        public RegisterResult(string message) : base(message)
        {
        }
    }
}
