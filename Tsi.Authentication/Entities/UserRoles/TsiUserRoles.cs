using System;
using System.Collections.Generic;
using System.Text;
using Tsi.Core.Entities.Auditing;

namespace Tsi.Authentication.Entities.UserRoles
{
    public class TsiUserRoles : IFullEntityObject
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public Guid RoleId { get; set; }

        public Guid? CreatorId { get; set; }

        public DateTime? CreationTime { get; set; }

        public Guid? LastModifierId { get; set; }

        public DateTime? LastModificationTime { get; set; }

        public Guid? DeleterId { get; set; }

        public DateTime? DeletionTime { get; set; }

        public bool IsDeleted { get; set; }

    }
}
