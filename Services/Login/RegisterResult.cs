namespace Application.Services.Login
{
    public class RegisterResult : RequestResult, ICaptchaResult
    {
        public string CaptchaErrorMessage => "Invalid captcha!";

        public bool CaptchaSuccess { get; set; }
        public RegisterResult()
        {
        }

        public RegisterResult(string message) : base(message)
        {
        }
    }
}
