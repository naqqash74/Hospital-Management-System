using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hospital_Management_System.Data.Config
{
    public class PatientConfig : IEntityTypeConfiguration<Patient>
    {
        public void Configure(EntityTypeBuilder<Patient> builder) 
        {
            builder.ToTable("Patients");
            builder.HasKey(x => x.PatientId);

            builder.Property(x =>x.PatientId).UseIdentityColumn();



            builder.HasOne(n => n.Person)
                .WithMany(b => b.Patients)
                .HasForeignKey(n => n.PersonId)
                .HasConstraintName("FK_Patient_Person_PersonId");
        }
    }
}
