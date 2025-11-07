using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Printstream.Attributes
{
    public class ArrayFormatAttribute : ValidationAttribute
    {
        private string _regex { get; }

        public ArrayFormatAttribute(string type)
        {
            var regex = type switch
            {
                "Address" => @"^(?:г\.)\s*[\p{L}\d\s\.\-]{2,}\s*,\s*(?:ул\.|пр-т\.)\s*[\p{L}\d\s\.\-]{2,}\s+[\p{L}\d\/\-]{1,}(?:,\s*стр\.\s*[\p{L}\d\/]+)?(?:,\s*(?:кв\.|оф\.|пом\.)\s*[\p{L}\d\/\-]+)?$",
                "Phone" => @"^\+7\(\d{3}\)\d{3}-\d{2}-\d{2}$",
                "Email" => @"^[a-zA-Z0-9._%+-]{3,}@[a-zA-Z0-9.-]{2,}\.[a-zA-Z]{2,6}$",
                _ => ""
            };

            _regex = regex;
            ErrorMessage = $"The field {type} is invalid:";
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                if (value is HashSet<string> metaData)
                {
                    foreach (var data in metaData)
                    {
                        if (!Regex.IsMatch(data, _regex) || string.IsNullOrWhiteSpace(data))
                        {
                            return new ValidationResult($"{ErrorMessage} {data}");
                        }
                    }
                }
            }

            return ValidationResult.Success!;
        }
    }
}