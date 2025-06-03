using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hospital_Management_System.Data.Config
{
    public class PersonConfig : IEntityTypeConfiguration<Person>
    {
        public void Configure(EntityTypeBuilder<Person> builder)
        {
            builder.ToTable("Persons");
            builder.HasKey(x => x.PersonId);

            builder.Property(x => x.PersonId).UseIdentityColumn();

            builder.Property(n => n.Name).IsRequired().HasMaxLength(100);
            builder.Property(n => n.PhoneNumber).IsRequired().HasMaxLength(20);
            builder.Property(n => n.Gender).IsRequired().HasMaxLength(1);
            builder.Property(n => n.Address).IsRequired().HasMaxLength(200);
            builder.Property(n => n.BirthDate).IsRequired();
            builder.Property(n => n.Email).IsRequired().HasMaxLength(100);

        }
    }
}
