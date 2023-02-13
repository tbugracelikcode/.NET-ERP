using System;
using System.Collections.Generic;
using System.Text;
using Tsi.Core.Entities.CreationEntites;
using Tsi.Core.Entities.DeleterEntities;
using Tsi.Core.Entities.ModifierEntities;

namespace Tsi.Core.Entities.Auditing
{
    public abstract class FullEntity : IFullEntityObject
    {
        public Guid Id { get; set; }
        public virtual Guid? CreatorId { get; set; }
        public virtual DateTime? CreationTime { get; set; }
        public virtual Guid? LastModifierId { get; set; }
        public virtual DateTime? LastModificationTime { get; set; }
        public virtual Guid? DeleterId { get; set; }
        public virtual DateTime? DeletionTime { get; set; }
        public bool IsDeleted { get; set; }
        public bool? DataOpenStatus { get; set; }
        public Guid? DataOpenStatusUserId { get; set; }
    }
}
