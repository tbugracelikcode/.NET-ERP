using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities;
using TSI.QueryBuilder.MappingAttributes;

namespace TsiErp.Entities.Entities.GeneralSystemIdentifications.UserPermission.Dtos
{
    public class UpdateUserPermissionsDto : IEntityDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Kullanıcı Id
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Menu Id
        /// </summary>
        public Guid MenuId { get; set; }

        /// <summary>
        /// Kullanıcı Yetkisi
        /// </summary>
        public bool IsUserPermitted { get; set; }

        [NoDatabaseAction]
        public List<SelectUserPermissionsDto> SelectUserPermissionsList { get; set; }
    }
}
