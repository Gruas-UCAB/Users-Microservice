using System.Text.RegularExpressions;

namespace UsersMicroservice.core.Common
{
    public static class EmailValidator
    {
        private static readonly Regex EmailRegExp = new Regex(
            @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
            RegexOptions.Compiled | RegexOptions.IgnoreCase
        );

        public static bool IsValid(string email)
        {
            if (string.IsNullOrEmpty(email))
                return false;
            return EmailRegExp.IsMatch(email);
        }
    }
}
