namespace Hospital_Management_System.Data
{
    public class Prescriptions
    {
        public int PrescriptionID { get; set; }
        public int MedicalRecordID { get; set; }
        public string MedicationName { get; set; }
        public string Dosage { get; set; }
        public string Frequency { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public string Instructions { get; set; }

        public Medical_Record MedicalRecord { get; set; }
    }
}
