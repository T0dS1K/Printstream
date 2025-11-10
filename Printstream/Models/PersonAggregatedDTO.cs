namespace Printstream.Models
{
    public class PersonAggregatedDTO
    {
        public int Person_ID { get; set; }
        public string? LastName { get; set; }
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? DateOfBirth { get; set; }
        public bool IsMale { get; set; }
        public string? Bunch_Value { get; set; }
        public List<string?> Addresses { get; set; } = new();
        public List<string?> Phones { get; set; } = new();
        public List<string?> Emails { get; set; } = new();
    }
}