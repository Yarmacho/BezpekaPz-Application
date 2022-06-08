namespace Application.Services.Login
{
    public interface ICaptchaResult
    {
        bool CaptchaSuccess { get; }

        string CaptchaErrorMessage { get; }
    }
}