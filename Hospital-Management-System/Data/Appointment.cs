namespace Hospital_Management_System.Data
{
    public class Appointment
    {
        public int AppointmentID { get; set; }
        public int PatientId { get; set; }
        public Patient Patient { get; set; } // Navigation property
        public int DoctorId { get; set; }
        public Doctor Doctor { get; set; } // Navigation property
        public int PaymentID { get; set; }
        public Payment Payment { get; set; } // Navigation property
        public int MedicalRecordID { get; set; }
        public Medical_Record MedicalRecord { get; set; } // Navigation property
        public DateOnly DateTime { get; set; }
        public byte AppointmentStatus { get; set; }
    }
}
