using System;
using System.Collections.Generic;
using System.Text;
using Tsi.Authentication.Dtos.Menus;
using Tsi.Core.Entities.Auditing;

namespace Tsi.Authentication.Dtos.RolePermissions
{
    public class SelectRolePermissionsDto : FullAuditedEntityDto
    {
        public Guid RoleId { get; set; }

        public string RoleName { get; set; }

        public List<ListMenusDto> Menus { get; set; }
    }
}
