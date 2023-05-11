using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NewScottApp.Domain.Domains.User;

namespace NewScottApp.Infrastructure.Configurations
{
    public class UserTokenConfigurations : IEntityTypeConfiguration<UserToken>
    {
        public void Configure(EntityTypeBuilder<UserToken> builder)
        {
            builder.ToTable("UserTokens", "IdentitySchema");
            builder.HasKey(x => new { x.UserId, x.LoginProvider });
        }
    }
}
