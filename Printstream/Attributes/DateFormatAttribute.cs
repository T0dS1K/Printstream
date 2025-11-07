using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace Printstream.Attributes
{
    public class DateFormatAttribute : ValidationAttribute
    {
        private const string dateFormat = "MM.dd.yyyy";

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is string dateString)
            {
                if (DateOnly.TryParseExact(dateString, dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateOnly dateOfBirth))
                {
                    if (DateOnly.FromDateTime(DateTime.Today).DayNumber >= dateOfBirth.DayNumber)
                    {
                        return ValidationResult.Success!;
                    }
                }
            }

            return new ValidationResult(ErrorMessage);
        }
    }
}