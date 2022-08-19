using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;

namespace Tsi.EntityFrameworkCore.Extensions
{
    public static class ChangeTrackerExtensions
    {
        public static void SetAuditProperties(this ChangeTracker changeTracker)
        {
            changeTracker.DetectChanges();

            IEnumerable<EntityEntry> entities = changeTracker.Entries().Where(t => t.Entity is IFullEntityObject && t.State == EntityState.Deleted);

            if (entities.Any())
            {
                foreach (EntityEntry entry in entities)
                {
                    IFullEntityObject entity = (IFullEntityObject)entry.Entity;
                    entity.IsDeleted = true;
                    entity.DeleterId = Guid.NewGuid();
                    entity.DeletionTime = DateTime.Now;
                    entry.State = EntityState.Modified;
                }
            }
        }
    }
}
