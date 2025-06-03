using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Hospital_Management_System.Data.Config
{
    public class AppointmentConfig : IEntityTypeConfiguration<Appointment>
    {
        public void Configure(EntityTypeBuilder<Appointment> builder)
        {
            // Table Name
            builder.ToTable("Appointment");

            // Primary Key
            builder.HasKey(x => x.AppointmentID);
            builder.Property(x => x.AppointmentID).UseIdentityColumn();

            // Properties
            builder.Property(x => x.DateTime).IsRequired();
            builder.Property(x => x.AppointmentStatus).IsRequired();

            // Relationships
            builder.HasOne(a => a.Patient)
                .WithMany(p => p.Appointments) // Explicit navigation property in Patient
                .HasForeignKey(a => a.PatientId)
                .HasConstraintName("FK_Appointment_Patient_PatientId")
                 .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(a => a.Doctor)
                .WithMany(d => d.Appointments) // Explicit navigation property in Doctor
                .HasForeignKey(a => a.DoctorId)
                .HasConstraintName("FK_Appointment_Doctor_DoctorId")
                 .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(a => a.MedicalRecord)
                .WithMany(m => m.Appointments) // Explicit navigation property in MedicalRecord
                .HasForeignKey(a => a.MedicalRecordID)
                .HasConstraintName("FK_Appointment_MedicalRecord_MedicalRecordId")
                 .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(a => a.Payment)
                .WithMany(p => p.Appointments) // Explicit navigation property in Payment
                .HasForeignKey(a => a.PaymentID)
                .HasConstraintName("FK_Appointment_Payment_PaymentId")
                 .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
