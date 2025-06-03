using System.ComponentModel.DataAnnotations;

namespace Hospital_Management_System.Models
{
    public class PatientDTO
    {
        [Required]
        public int PatientId { get; set; }
        [Required]
        public int PersonId { get; set; }
    }
}
