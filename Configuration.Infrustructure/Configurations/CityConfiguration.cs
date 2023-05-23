using Configuration.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Configuration.Infrastructure.Configurations
{
    public class CityConfiguration : IEntityTypeConfiguration<City>
    {
        public void Configure(EntityTypeBuilder<City> builder)
        {
            builder.ToTable("City", "dbo");
            builder.Property<string>(x=>x.CityNameEnglish).HasMaxLength(50);
            builder.Property(p=>p.CityNameArabic).HasMaxLength(30);
            builder.HasOne(x => x.Country)
                .WithMany(x => x.City)
                ;
            builder.HasOne(x => x.Region)
               .WithMany(x => x.City);

        }
    }
}
