namespace Hospital_Management_System.Data
{
    public class Doctor
    {
        public int DoctorId { get; set; }
        public string Specialization { get; set; }
        public int PersonId { get; set; }

        // Navigation Property for Person
        public virtual Person Person { get; set; }

        public ICollection<Appointment> Appointments { get; set; }
    }
}
