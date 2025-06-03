using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hospital_Management_System.Data.Config
{
    public class PrescriptionsConfig :  IEntityTypeConfiguration<Prescriptions>
    {
        public void Configure(EntityTypeBuilder<Prescriptions> builder)
    {
            builder.ToTable("Prescriptions");
            builder.HasKey(x => x.PrescriptionID);

            builder.Property(x => x.PrescriptionID).UseIdentityColumn();


            builder.Property(n => n.MedicationName).IsRequired().HasMaxLength(100);
            builder.Property(n => n.Dosage).IsRequired().HasMaxLength(50);
            builder.Property(n => n.Frequency).IsRequired().HasMaxLength(50);
            builder.Property(n => n.StartDate).IsRequired();
            builder.Property(n => n.EndDate).IsRequired();
            builder.Property(n => n.Instructions).IsRequired().HasMaxLength(200);


            // Foreign Key Configuration: MedicalRecord -> Prescription
            builder.HasOne(p => p.MedicalRecord)
                .WithMany(mr => mr.Prescriptions)
                .HasForeignKey(p => p.MedicalRecordID)
                .IsRequired(false);

        }
    }

}
