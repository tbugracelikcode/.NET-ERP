using System;
using System.Collections.Generic;
using System.Text;
using Tsi.Authentication.Entities.RolePermissions;
using Tsi.Core.Entities;
using Tsi.Core.Entities.Auditing;

namespace Tsi.Authentication.Entities.Roles
{
    public class TsiRoles : IFullEntityObject
    {
        public Guid Id { get; set; }

        public string RoleName { get; set; }

        public Guid? CreatorId { get; set; }

        public DateTime? CreationTime { get; set; }

        public Guid? LastModifierId { get; set; }

        public DateTime? LastModificationTime { get; set; }

        public Guid? DeleterId { get; set; }

        public DateTime? DeletionTime { get; set; }

        public bool IsDeleted { get; set; }

        public ICollection<TsiRolePermissions> TsiRolePermissions { get; set; }
    }
}
