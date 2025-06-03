using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Hospital_Management_System.Data.Config
{
    public class PaymentConfig : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.ToTable("Payment");
            builder.HasKey(x => x.PaymentID);

            builder.Property(x => x.PaymentID).UseIdentityColumn();


            builder.Property(n => n.PaymentMethod).IsRequired().HasMaxLength(50);
            builder.Property(n => n.Amount).IsRequired();
            builder.Property(n => n.Date).IsRequired();
            builder.Property(n => n.Notes).IsRequired().HasMaxLength(200);
            builder.Property(n => n.Amount).HasColumnType("decimal(8,2)").IsRequired();


          builder.HasOne(p => p.Patient) 
                 .WithMany(p => p.Payment) 
                 .HasForeignKey(p => p.PatientId); 

        }
    }
}
