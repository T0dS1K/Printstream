using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Printstream.Attributes
{
    public class ValueFormatAttribute : ValidationAttribute
    {
        private string _regex { get; set; }
        private const string _regexA = @"^(?:г\.)\s*[\p{L}\d\s\.\-]{2,}\s*,\s*(?:ул\.|пр-т\.)\s*[\p{L}\d\s\.\-]{2,}\s+[\p{L}\d\/\-]{1,}(?:,\s*стр\.\s*[\p{L}\d\/]+)?(?:,\s*(?:кв\.|оф\.|пом\.)\s*[\p{L}\d\/\-]+)?$";
        private const string _regexP = @"^\+7\(\d{3}\)\d{3}-\d{2}-\d{2}$";
        private const string _regexE = @"^[a-zA-Z0-9._%+-]{3,}@[a-zA-Z0-9.-]{2,}\.[a-zA-Z]{2,6}$";

        public ValueFormatAttribute(string type)
        {
            var regex = type switch
            {
                "Address" => _regexA,
                "Phone" => _regexP,
                "Email" => _regexE,
                _ => ""
            };

            _regex = regex;
            ErrorMessage = $"The field {type} is invalid:";
        }

        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            if (value != null)
            {
                if (value is HashSet<string> HSРData)
                {
                    foreach (var data in HSРData)
                    {
                        if (!Regex.IsMatch(data, _regex) || string.IsNullOrWhiteSpace(data))
                        {
                            return new ValidationResult($"{ErrorMessage} {data}");
                        }
                    }
                }
                else if (value is string SData)
                {
                    if (Regex.IsMatch(SData, _regexA))
                    {
                        _regex = _regexA;
                        return ValidationResult.Success!;
                    }

                    if (Regex.IsMatch(SData, _regexP))
                    {
                        _regex = _regexP;
                        return ValidationResult.Success!;
                    }

                    if (Regex.IsMatch(SData, _regexE))
                    {
                        _regex = _regexE;
                        return ValidationResult.Success!;
                    }

                    return new ValidationResult($"{ErrorMessage} {SData}");
                }
            }

            return ValidationResult.Success!;
        }

        public new string GetType()
        {
            var type = _regex switch
            {
                _regexA => "Address",
                _regexP => "Phone",
                _regexE => "Email",
                _ => ""
            };

            return type;
        }
    }
}