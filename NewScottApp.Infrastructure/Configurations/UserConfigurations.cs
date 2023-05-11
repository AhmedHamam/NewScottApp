using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NewScottApp.Domain.Domains.User;

namespace NewScottApp.Infrastructure.Configurations
{
    public class UserConfigurations : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.ToTable("Users", "IdentitySchema");
            builder.HasKey(x => x.Id);

            // Each User can have many entries in the UserRole join table
            //builder.HasMany(e => e.UserRoles)
            //    .HasForeignKey(ur => ur.UserId)
            //    .IsRequired();

            builder.HasMany(e => e.Claims)
                .WithOne(claim => claim.User)
                .IsRequired();
        }
    }
}
