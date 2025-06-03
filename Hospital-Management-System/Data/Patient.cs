namespace Hospital_Management_System.Data
{
    public class Patient
    {
        public int PatientId { get; set; }
        public int PersonId { get; set; }

        // Navigation Property for Person
        public virtual Person Person { get; set; }

        public ICollection<Payment> Payment { get; set; }
        public ICollection<Appointment> Appointments { get; set; }
    }
}
