using System;
using System.Collections.Generic;
using System.Text;
using Tsi.Core.Entities.Auditing;

namespace Tsi.Core.Entities.Auditing
{
    public abstract class FullAuditedEntityDto : FullEntityDto
    {
        public Guid CreatorId { get; set; }

        public DateTime CreationTime { get; set; }

        public Guid? LastModifierId { get; set; }

        public DateTime? LastModificationTime { get; set; }

        public Guid? DeleterId { get; set; }

        public DateTime? DeletionTime { get; set; }

        public bool? DataOpenStatus { get; set; }

        public Guid? DataOpenStatusUserId { get; set; }

        public bool IsDeleted { get; set; }
    }
}
