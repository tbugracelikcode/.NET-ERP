using System;
using System.Collections.Generic;
using System.Text;

namespace Tsi.Core.Entities.CreationEntites
{
    public interface ICreationEntity : ICreationTime
    {
        Guid? CreatorId { get; set; }
    }
}
