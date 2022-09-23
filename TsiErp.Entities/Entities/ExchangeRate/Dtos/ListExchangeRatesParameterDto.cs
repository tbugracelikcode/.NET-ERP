using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.ExchangeRate.Dtos
{
    public class ListExchangeRatesParameterDto : FullAuditedEntityDto
    {
        public bool IsActive { get; set; } = true;
    }
}
