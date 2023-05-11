using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NewScottApp.Domain.Domains.User;

namespace NewScottApp.Infrastructure.Configurations
{

    public class UserLoginConfigurations : IEntityTypeConfiguration<UserLogin>
    {
        public void Configure(EntityTypeBuilder<UserLogin> builder)
        {
            builder.ToTable("UserLogins", "IdentitySchema");
            builder.HasKey(x => x.Id);
            builder.HasOne(x => x.User).WithMany(x => x.UserLogins)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
