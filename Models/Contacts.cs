using System.ComponentModel.DataAnnotations;

namespace Sistema8QUALY.Models
{
    public class Contacts
    {
        [Key]
        public int ContactId { get; set; }

        [Required]
        public string? Name { get; set; }

        public string? Company { get; set; }

        public List<string> Emails { get; set; } = new List<string>();

        public string? PersonalPhone { get; set; }

        public string? BusinessPhone { get; set; }
    }
}
