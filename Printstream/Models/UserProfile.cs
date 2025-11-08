namespace Printstream.Models
{
    public class UserProfile
    {
        public string? LastName { get; set; }
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? DateOfBirth { get; set; }
        public bool IsMale { get; set; }
        public HashSet<string>? Addresses { get; set; }
        public HashSet<string>? Phones { get; set; }
        public HashSet<string>? Emails { get; set; }

        public UserProfile() { }
        public UserProfile(UserDTO DTO)
        {
            LastName = DTO.LastName;
            FirstName = DTO.FirstName;
            MiddleName = DTO.MiddleName;
            DateOfBirth = DTO.DateOfBirth;
            IsMale = DTO.IsMale;
            Addresses = DTO.Addresses;
            Phones = DTO.Phones;
            Emails = DTO.Emails;
        }
    }
}