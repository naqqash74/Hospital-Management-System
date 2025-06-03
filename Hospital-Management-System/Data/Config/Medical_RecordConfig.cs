using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Hospital_Management_System.Data.Config
{
    public class Medical_RecordConfig : IEntityTypeConfiguration<Medical_Record>
    {
        public void Configure(EntityTypeBuilder<Medical_Record> builder)
        {
            builder.ToTable("Medical_Record");
            builder.HasKey(x => x.MedicalRecordID);

            builder.Property(x => x.MedicalRecordID).UseIdentityColumn();

            builder.Property(n => n.Description).IsRequired().HasMaxLength(200);
            builder.Property(n => n.Diagnosis).IsRequired().HasMaxLength(200);
            builder.Property(n => n.Notes).IsRequired().HasMaxLength(200);

            builder.HasData(new List<Medical_Record>()
            {
                new Medical_Record
                {
                    MedicalRecordID = 1,
                    Description = "Bed rest",
                    Diagnosis = "ABC",
                    Notes = "GHI",
                },
                 new Medical_Record
                {
                    MedicalRecordID = 2,
                    Description = "walk early morning",
                    Diagnosis = "DEF",
                    Notes = "DEF",
                },
                  new Medical_Record
                {
                    MedicalRecordID = 3,
                    Description = "dont take stress",
                    Diagnosis = "GHI",
                    Notes = "ABC",
                }
            });

        }
    }
}
