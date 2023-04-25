using System;
using System.Collections.Generic;
using System.Text;

namespace Tsi.Core.Entities.ModifierEntities
{
    public interface IModificationEntity : IModificationTime
    {
        Guid? LastModifierId { get; set; }
    }
}
