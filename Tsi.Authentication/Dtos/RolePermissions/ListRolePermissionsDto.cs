using System;
using System.Collections.Generic;
using System.Text;
using Tsi.Core.Entities.Auditing;

namespace Tsi.Authentication.Dtos.RolePermissions
{
    public class ListRolePermissionsDto : FullAuditedEntityDto
    {
        public string RoleName { get; set; }
    }
}
