using System;
using System.Collections.Generic;
using System.Text;

namespace Tsi.Core.Entities.Auditing
{
    public abstract class FullEntityDto : IFullEntityDtoObject
    {
        public Guid Id { get; set; }
    }
}
