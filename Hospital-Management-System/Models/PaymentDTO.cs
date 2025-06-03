namespace Hospital_Management_System.Models
{
    public class PaymentDTO
    {
        public int PaymentID { get; set; }
        public int PatientId { get; set; }
        public DateOnly Date { get; set; }
        public string PaymentMethod { get; set; }
        public decimal Amount { get; set; }
        public string Notes { get; set; }
    }
}
