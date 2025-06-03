namespace Hospital_Management_System.Data
{
    public class Medical_Record
    {
        public int MedicalRecordID { get; set; }
        public string Description { get; set; }
        public string Diagnosis { get; set; }
        public string Notes { get; set; }

        public ICollection<Prescriptions> Prescriptions { get; set; }
        public ICollection<Appointment> Appointments { get; set; }
    }
}
