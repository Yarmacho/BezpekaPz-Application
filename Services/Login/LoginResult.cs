namespace Application.Services.Login
{
    public class LoginResult : RequestResult
    {
        public string Login { get; set; }

        public LoginResult() : base()
        {
        }

        public LoginResult(string message) : base(message)
        {
        }
    }
}
