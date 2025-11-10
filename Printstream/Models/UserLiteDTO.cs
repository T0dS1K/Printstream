using Printstream.Attributes;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Printstream.Models
{
    public class UserLiteDTO
    {
        [Required]
        [NameFormat]
        [DefaultValue("")]
        [Description("Фамилия")]
        public string? LastName { get; set; }

        [NameFormat]
        [DefaultValue("")]
        [Description("Имя")]
        public string? FirstName { get; set; }

        [NameFormat]
        [DefaultValue("")]
        [Description("Отчество")]
        public string? MiddleName { get; set; }
    }
}
