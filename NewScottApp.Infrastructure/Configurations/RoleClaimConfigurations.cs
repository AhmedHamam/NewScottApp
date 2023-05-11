using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NewScottApp.Domain.Domains.User;

namespace NewScottApp.Infrastructure.Configurations
{
    public class RoleClaimConfigurations : IEntityTypeConfiguration<RoleClaim>
    {
        public void Configure(EntityTypeBuilder<RoleClaim> builder)
        {
            builder.ToTable("RoleClaims", "IdentitySchema");
            builder.HasKey(x => x.Id);
            builder.HasOne(x => x.Role).WithMany(x => x.RoleClaims)
                .HasForeignKey(x => x.RoleId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
