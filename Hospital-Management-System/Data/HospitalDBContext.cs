using Hospital_Management_System.Data.Config;
using Microsoft.EntityFrameworkCore;

namespace Hospital_Management_System.Data
{
    public class HospitalDBContext : DbContext
    {
        public HospitalDBContext(DbContextOptions<HospitalDBContext> options) : base(options)
        {
            
        }
        public  DbSet<Person>  Persons {  get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Medical_Record> Medical_Record { get; set; }
        public DbSet<Prescriptions> Prescriptions { get; set; }
        public DbSet<Payment> Payment { get; set; }
        public DbSet<Appointment> Appointment { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Table 1
            modelBuilder.ApplyConfiguration(new PersonConfig());
            // Table 2
            modelBuilder.ApplyConfiguration(new PatientConfig());
            // Table 3
            modelBuilder.ApplyConfiguration(new DoctorConfig());
            // Table 4
            modelBuilder.ApplyConfiguration(new Medical_RecordConfig());
            // Table 5
            modelBuilder.ApplyConfiguration(new PrescriptionsConfig());
            // Table 6
            modelBuilder.ApplyConfiguration(new PaymentConfig());
            // Table 7
            modelBuilder.ApplyConfiguration(new AppointmentConfig());
        }
    }
}

