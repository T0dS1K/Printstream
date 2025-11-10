using System.ComponentModel;
using Printstream.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Printstream.Models
{
    public class UserDTO
    {
        [Required]
        [NameFormat]
        [DefaultValue("")]
        [Description("Фамилия")]
        public string? LastName { get; set; } 

        [Required]
        [NameFormat]
        [DefaultValue("")]
        [Description("Имя")]
        public string? FirstName { get; set; }

        [NameFormat]
        [DefaultValue("")]
        [Description("Отчество")]
        public string? MiddleName { get; set; }

        [Required]
        [DateFormat]
        [DefaultValue("")]
        [Description("Пример: ММ.ДД.ГГГГ")]
        public string? DateOfBirth { get; set; }

        [Required]
        [Description("Пол")]
        public bool IsMale { get; set; }

        [Required]
        [ValueFormat("Address")]
        [Description("Пример: г. Москва, ул. Земляной Вал 50А | г. Санкт-Петербург, пр-т. Невский 15/2 | г. Казань, ул. Толстого 10Б, стр. 5 | г. Новосибирск, пр-т. Мира 2, кв. 101 | г. Курск, ул. Ленина 30-1, оф. 2 | г. Томск, ул. Студенческая 15, пом. 7/2")]
        public HashSet<string>? Addresses { get; set; }

        [ValueFormat("Phone")]
        [Description("Пример: +7(XXX)XXX-XX-XX")]
        public HashSet<string>? Phones { get; set; }

        [ValueFormat("Email")]
        [Description("Пример: mail@example.com")]
        public HashSet<string>? Emails { get; set; }
    }
}