namespace Application.Services.Login
{
    public class RestoreResult : RequestResult, ICaptchaResult
    {
        public string CaptchaErrorMessage => "Invalid captcha!";

        public bool CaptchaSuccess { get; set; }
        public RestoreResult()
        {
        }

        public RestoreResult(string message) : base(message)
        {
        }
    }
}
