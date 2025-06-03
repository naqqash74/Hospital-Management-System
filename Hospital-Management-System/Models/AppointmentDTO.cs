namespace Hospital_Management_System.Models
{
    public class AppointmentDTO
    {
        public int AppointmentID { get; set; }
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public int PaymentID { get; set; }
        public int MedicalRecordID { get; set; }
        public DateOnly DateTime { get; set; }
        public byte AppointmentStatus { get; set; }
    }
}
