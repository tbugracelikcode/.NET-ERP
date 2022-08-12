using System;
using System.Collections.Generic;
using System.Text;

namespace Tsi.Core.Entities.ModifierEntities
{
    public interface IModificationTime
    {
        DateTime? LastModificationTime { get; }
    }
}
