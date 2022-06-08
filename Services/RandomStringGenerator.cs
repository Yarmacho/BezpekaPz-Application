using System;
using System.Text;

namespace Application.Services
{
    public class RandomStringGenerator
    {
        private readonly string _symbols;
        private readonly Random _rnd;

        public RandomStringGenerator()
        {
            var sb = new StringBuilder();
            for (char i = 'a'; i <= 'z'; i++)
            {
                sb.Append(i);
                sb.Append(char.ToUpper(i));
            }
            sb.Append("0123456789");
            _symbols = sb.ToString();
            _rnd = new Random((int)DateTime.Now.Ticks);
        }

        public string Generate(int len)
        {
            var sb = new StringBuilder();
            for (var i = 0; i < len; i++)
            {
                sb.Append(_symbols[_rnd.Next(0, _symbols.Length)]);
            }
            return sb.ToString();
        }
    }
}
