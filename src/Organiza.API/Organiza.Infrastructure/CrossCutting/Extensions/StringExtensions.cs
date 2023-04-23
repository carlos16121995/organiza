using System.Text;
using System.Text.RegularExpressions;

namespace Organiza.Infrastructure.CrossCutting.Extensions
{
    public static class StringExtensions
    {
        public static bool IsGuid(this string value)
        {
            return Guid.TryParse(value, out _);
        }

        public static string? ConvertUtf8ToBase64(this string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return null;

            byte[] valueInBytes = Encoding.UTF8.GetBytes(value);
            return Convert.ToBase64String(valueInBytes);
        }
        public static string? ConvertUtf8ToBase64(this string clientId, string clientSecret)
        {
            if (string.IsNullOrWhiteSpace(clientId) || string.IsNullOrWhiteSpace(clientSecret)) return null;

            var key = string.Format("{0}:{1}", clientId, clientSecret);

            byte[] valueInBytes = Encoding.UTF8.GetBytes(key);
            return Convert.ToBase64String(valueInBytes);
        }


        public static bool IsValidIp(this string ip)
        {
            Regex rx = new Regex(@"\b(?:\d{1,3}\.){3}\d{1,3}\b");

            return rx.IsMatch(ip);
        }

        public static string FormatHolderName(this string name)
        {
            string holderName = name.ToUpper();

            if (!string.IsNullOrEmpty(holderName))
            {
                holderName = Regex.Replace(holderName, "[ÀÁÂÃ]", "A");
                holderName = Regex.Replace(holderName, "[ÈÉÊ]", "E");
                holderName = Regex.Replace(holderName, "[ÌÍ]", "I");
                holderName = Regex.Replace(holderName, "[ÒÓÔÕ]", "O");
                holderName = Regex.Replace(holderName, "[ÙÚÛÜ]", "U");
                holderName = Regex.Replace(holderName, "[Ç]", "C");
            }

            return holderName;
        }

        public static bool IsValidCpf(this string data)
        {
            if (data.Length != 11)
                return false;

            else if (!IsNumbersOnly(data))
                return false;

            var cpf = data.Substring(0, 9);
            var cpfChar = cpf.ToCharArray();
            var verificationCode = data.Substring(9, 2);
            var sum = 0;
            var position = 0;

            for (int i = cpfChar.Length; i > 0; i--)
            {
                sum += Convert.ToInt32(cpfChar[position].ToString()) * (i + 1);
                position++;
            }

            int digit = 11 - sum % 11 > 9 ? 0 : 11 - sum % 11;

            cpf = string.Concat(cpf, digit);

            cpfChar = cpf.ToCharArray();
            position = 0;
            sum = 0;

            for (int i = cpfChar.Length; i > 0; i--)
            {
                sum += Convert.ToInt32(cpfChar[position].ToString()) * (i + 1);
                position++;
            }

            digit = 11 - (sum % 11) > 9 ? 0 : 11 - (sum % 11);

            cpf = string.Concat(cpf, digit);

            return cpf.Substring(9, 2) == verificationCode;
        }

        public static bool IsValidCnpj(this string data)
        {
            if (data.Length != 14)
                return false;

            else if (!IsNumbersOnly(data))
                return false;

            var cnpj = data.Substring(0, 12);
            var cnpjArray = cnpj.ToCharArray();
            var verificationCode = data.Substring(12, 2);
            var sum = 0;
            int digit;

            var firstWeight = new int[] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            var secondWeight = new int[] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

            for (int i = 0; i < cnpjArray.Length; i++)
                sum += Convert.ToInt32(cnpjArray[i].ToString()) * firstWeight[i];

            digit = sum % 11 < 2 ? 0 : 11 - (sum % 11);

            cnpj = string.Concat(cnpj, digit);
            cnpjArray = cnpj.ToCharArray();

            sum = 0;

            for (int i = 0; i < cnpjArray.Length; i++)
                sum += Convert.ToInt32(cnpjArray[i].ToString()) * secondWeight[i];

            digit = sum % 11 < 2 ? 0 : 11 - (sum % 11);

            cnpj = string.Concat(cnpj, digit);

            return cnpj.Substring(12, 2) == verificationCode;
        }

        public static bool IsNumbersOnly(this string value)
        {
            Regex rx = new Regex(@"^[0-9]+$");

            return rx.IsMatch(value);
        }

        public static bool IsValidName(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return false;

            Regex rx = new Regex(@"[^A-Za-z\sÄÅÁÂÀÃäáâàãÉÊËÈéêëèÍÎÏÌíîïìÖÓÔÒÕöóôòõÜÚÛÙüúûùÇç]");

            return !rx.IsMatch(value);
        }

        public static bool IsValidCustumerName(this string value)
        {
            return value?.TrimStart().TrimEnd().Split(" ").Length > 1;
        }

        public static bool IsValidEmail(this string value)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(value);
                return addr.Address == value;
            }
            catch
            {
                return false;
            }
        }

        public static string BuildMaskedCardNumber(this string cardNumber)
        {
            string maskedCardNumber = string.Empty;
            string _cardNumber = cardNumber.Replace(" ", string.Empty).Replace("-", string.Empty);

            if (!string.IsNullOrEmpty(_cardNumber) && _cardNumber.Length > 10)
            {
                maskedCardNumber = $"{_cardNumber.Substring(0, 6)}{String.Join("", Enumerable.Repeat('*', _cardNumber.Length - 10).ToArray())}{_cardNumber.Substring(_cardNumber.Length - 4, 4)}";
            }

            return maskedCardNumber;
        }

        public static string RemoveDiacritics(this string text)
        {
            string comAcentos = "ÄÅÁÂÀÃäáâàãÉÊËÈéêëèÍÎÏÌíîïìÖÓÔÒÕöóôòõÜÚÛÙüúûùÇç";
            string semAcentos = "AAAAAAaaaaaEEEEeeeeIIIIiiiiOOOOOoooooUUUUuuuuCc";

            for (int i = 0; i < comAcentos.Length; i++)
            {
                text = text.Replace(comAcentos[i].ToString(), semAcentos[i].ToString());
            }
            return text;
        }
    }
}
