using System;
using System.Collections.Generic;
using System.Text;

namespace Tsi.Core.Entities.DataConcurrencyEntities
{
    public interface IDataConcurrencyStamp
    {
        bool? DataOpenStatus { get; set; }

        Guid? DataOpenStatusUserId { get; set; }
    }
}
