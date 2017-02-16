using System;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace UIDGenerator
{
    public sealed class UniqueIdGenerator
    {
        private readonly object _lock = new object();
        private const int MaxLength = 6;

        public string GetUniqueIdentifier(string input)
        {
            var stringBuilder = new StringBuilder();

            lock (_lock)
            {
                var hashString = GetHash(input);
                foreach (char chr in hashString)
                {
                    if(!stringBuilder.ToString().Contains(chr) && chr != '0' && chr != 'o' && chr != '1' && chr != 'I')
                        stringBuilder.Append(chr);
                    if (stringBuilder.ToString().Length == MaxLength)
                        break;
                }
            }
            return stringBuilder.ToString();
        }

        private string GetHash(string input)
        {
            if (String.IsNullOrEmpty(input))
                return String.Empty;

            using (var sha = new SHA256Managed())
            {
                byte[] inputData = Encoding.UTF8.GetBytes(input);
                byte[] hash = sha.ComputeHash(inputData);
                return BitConverter.ToString(hash).Replace("-", String.Empty);
            }
        }

        #region other
        public string GetGuid(int id)
        {
            var now = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
            string input = string.Concat(id, now).Replace(" ", "");
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(input));
        }

        public string GetBase62UniqueIdentifier(string name, string dob)
        {
            var now = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
            string input = string.Concat(name, dob, now).Replace(" ", "");
            var stringBuilder = new StringBuilder();

            lock (_lock)
            {
                var hashString = GetHash(input);
                string base62Text = GetBase62(hashString);
                foreach (char chr in base62Text)
                {
                    if (!stringBuilder.ToString().Contains(chr) && chr != '0' && chr != 'o' && chr != '1' && chr != 'I')
                        stringBuilder.Append(chr);
                    if (stringBuilder.ToString().Length == MaxLength)
                        break;
                }
            }
            return stringBuilder.ToString();
        }

        private static Random _random = new Random();

        private static string GetBase62(string inputText)
        {
            var sb = new StringBuilder();

            for (int i = 0; i < inputText.Length; i++)
                sb.Append(inputText[_random.Next(62)]);

            return sb.ToString();
        }

        #endregion
    }
}
