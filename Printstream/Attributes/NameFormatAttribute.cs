using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Printstream.Attributes
{
    public class NameFormatAttribute : ValidationAttribute
    {
        private const string _regex = @"^[А-ЯЁ][а-яё]{1,}(?:-[А-ЯЁ][а-яё]{1,})*$";

        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            if (value != null)
            {
                var name = value.ToString();

                if (!Regex.IsMatch(name!, _regex) || name!.Split('-').Length > 2)
                {
                    return new ValidationResult(ErrorMessage);
                }
            }

            return ValidationResult.Success!;
        }
    }
}