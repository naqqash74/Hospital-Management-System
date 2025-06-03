using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Hospital_Management_System.Data.Config
{
    public class DoctorConfig : IEntityTypeConfiguration<Doctor>
    {
        public void Configure(EntityTypeBuilder<Doctor> builder)
        {
            builder.ToTable("Doctor");
            builder.HasKey(x => x.DoctorId);

            builder.Property(x => x.DoctorId).UseIdentityColumn();

            builder.Property(x => x.Specialization).IsRequired().HasMaxLength(100);



            builder.HasOne(n => n.Person)
                .WithMany(b => b.Doctors)
                .HasForeignKey(n => n.PersonId)
                .HasConstraintName("FK_Doctor_Person_PersonId");
        }
    }

}
