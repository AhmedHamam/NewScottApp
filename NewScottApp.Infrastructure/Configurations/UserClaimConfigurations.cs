using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NewScottApp.Domain.Domains.User;

namespace NewScottApp.Infrastructure.Configurations
{
    public class UserClaimConfigurations : IEntityTypeConfiguration<UserClaim>
    {
        public void Configure(EntityTypeBuilder<UserClaim> builder)
        {
            builder.ToTable("UserClaims", "IdentitySchema");
            builder.HasKey(x => x.Id);
            builder.HasOne(x => x.User).WithMany(x => x.Claims)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
