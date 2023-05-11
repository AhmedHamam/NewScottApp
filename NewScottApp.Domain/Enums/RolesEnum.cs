using System.ComponentModel;

namespace NewScottApp.Domain.Enums
{
    public enum RolesEnum
    {
        [Description("Group Admin")]
        GroupAdmin = 1,
        [Description("Group Member")]
        GroupMember = 2,
        [Description("User")]
        User = 3
    }
}
