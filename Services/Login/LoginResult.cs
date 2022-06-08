namespace Application.Services.Login
{
    public class LoginResult : RequestResult, ICaptchaResult
    {
        public string Login { get; set; }

        public string CaptchaErrorMessage => "Invalid captcha!";

        public bool CaptchaSuccess { get; set; }

        public LoginResult() : base()
        {
        }

        public LoginResult(string message) : base(message)
        {
        }
    }
}
