using System.ComponentModel.DataAnnotations;

namespace Hospital_Management_System.Models
{
    public class PersonDTO
    {
        public int PersonId { get; set; }
        public string Name { get; set; }
        public DateOnly BirthDate { get; set; }
        public string Gender { get; set; }
        public string PhoneNumber { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string Address { get; set; }
    }
}
