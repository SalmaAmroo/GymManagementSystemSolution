using GymManagementDAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace GymManagement_DAL_.Data.Configurations
{
    internal class GymUserConfiguration<T> : IEntityTypeConfiguration<T> where T : GymUser
    {
        public void Configure(EntityTypeBuilder<T> builder)
        {
            builder.Property(GU => GU.Name)
                   .HasColumnType("varchar")
                   .HasMaxLength(50);

            builder.Property(GU => GU.Email)
                   .HasColumnType("varchar")
                   .HasMaxLength(100);
            builder.Property(GU => GU.PhoneNumber)
                   .HasColumnType("varchar")
                   .HasMaxLength(11);
            builder.ToTable(Tb =>
            {
                Tb.HasCheckConstraint("GymUserValidEmailCheck", " Email Like '_%@_%._%'");
                Tb.HasCheckConstraint("GymUserValidPhoneNumberCheck", " PhoneNumber Like '01%' AND PhoneNumber NOT LIKE '%[^0-9]%' ");
            });
            builder.HasIndex(GU => GU.Email)
                   .IsUnique();
            builder.HasIndex(GU=>GU.PhoneNumber)
                   .IsUnique();
            builder.OwnsOne(GU => GU.Address, AddressBuilder =>
            {
                AddressBuilder.Property(A => A.Street)
                              .HasColumnName("Street")
                              .HasColumnType("Varchar")
                              .HasMaxLength(30);
                AddressBuilder.Property(A => A.City)
                              .HasColumnName("City")
                              .HasColumnType("Varchar")
                              .HasMaxLength(30);
                AddressBuilder.Property(A => A.BuildNumber)
                              .HasColumnName("BuidNumber");
                              


            });
        }
    }
}
