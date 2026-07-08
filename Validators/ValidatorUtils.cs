using System.Text.RegularExpressions;

namespace APIKros.Validators
{
    // Utility for Validation 
    public static class ValidationUtils
    {
        private static readonly string[] AllowedTitles =
        {
        "Bc.", "Mgr.", "Ing.", "Ing. arch.", "MUDr.", "MVDr.",
        "JUDr.", "PhDr.", "RNDr.", "PaedDr.", "PharmDr.",
        "doc.", "prof.", "PhD.", "MBA"
    };

        public static bool IsValidPhone(string? phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
                return false;

            return Regex.IsMatch(phone, @"^\+?[0-9\s\-\/]{7,30}$");
        }

        public static bool IsAllowedTitle(string? title)
        {
            if (string.IsNullOrWhiteSpace(title))
                return true;

            return AllowedTitles.Contains(title);

        }
    }

}