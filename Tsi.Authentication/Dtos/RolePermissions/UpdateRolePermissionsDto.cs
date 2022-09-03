using System;
using System.Collections.Generic;
using System.Text;
using Tsi.Authentication.Dtos.Menus;
using Tsi.Core.Entities.Auditing;

namespace Tsi.Authentication.Dtos.RolePermissions
{
    public class UpdateRolePermissionsDto : FullAuditedEntityDto
    {
        public string RoleName { get; set; }

        public List<ListMenusDto> Menus { get; set; }
    }
}
