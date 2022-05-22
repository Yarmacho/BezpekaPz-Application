namespace Application.Services.Encryption
{
    public interface IEncryptionService
    {
        string Encrypt(string input, string key);
        string Decrypt(string input, string key);
    }
}
