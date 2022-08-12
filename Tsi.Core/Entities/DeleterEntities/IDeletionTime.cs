using System;
using System.Collections.Generic;
using System.Text;

namespace Tsi.Core.Entities.DeleterEntities
{
    public interface IDeletionTime
    {
        DateTime? DeletionTime { get; set; }
    }
}
