using TsiErp.Entities.Entities.GeneralSystemIdentifications.UserPermission.Dtos;

namespace TsiErp.DataAccess.Services.Login
{
    public static class LoginedUserService
    {
        public static Guid UserId { get; set; }
        public static Guid VersionTableId { get; set; }
    }
}
