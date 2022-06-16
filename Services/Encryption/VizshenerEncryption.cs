using System.Text;

namespace Application.Services.Encryption
{
    public class VizshenerEncryption : IEncryptionService
    {
        private const string _alphabet = " abcdefghijklmnopqrstuvwxyz";

        private static string getRepeatKey(string input, int inputLen)
        {
            var password = new StringBuilder(input);
            while (password.Length < inputLen)
            {
                password.Append(input);
            }

            return password.ToString().Substring(0, inputLen);
        }

        public string Decrypt(string input, string key)
        {
            return vigenere(input, key, false);
        }

        public string Encrypt(string input, string key)
        {
            return vigenere(input, key, true);
        }
        private static string vigenere(string text, string password, bool encrypting = true)
        {
            var gamma = getRepeatKey(password, text.Length);
            var retValue = new StringBuilder();
            var q = _alphabet.Length;

            for (int i = 0; i < text.Length; i++)
            {
                var letterIndex = _alphabet.IndexOf(char.ToLowerInvariant(text[i]));
                var codeIndex = _alphabet.IndexOf(char.ToLowerInvariant(gamma[i]));
                if (letterIndex < 0)
                {
                    retValue.Append(text[i]);
                }
                else
                {
                    retValue.Append(_alphabet[(q + letterIndex + ((encrypting ? 1 : -1) * codeIndex)) % q]);
                }
            }

            return retValue.ToString();
        }
    }
}
