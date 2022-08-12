using System;
using System.Collections.Generic;
using System.Text;

namespace Tsi.Core.Entities.DeleterEntities
{
    public interface IDeletionEntity : IDeletionTime
    {
        Guid? DeleterId { get; set; }
    }
}
