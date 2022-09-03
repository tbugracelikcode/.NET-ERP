using System;
using System.Collections.Generic;
using System.Text;
using Tsi.Core.Entities.Auditing;

namespace Tsi.Authentication.Dtos.Roles
{
    public class UpdateRolesDto : FullAuditedEntityDto
    {
        public string RoleName { get; set; }
    }
}
