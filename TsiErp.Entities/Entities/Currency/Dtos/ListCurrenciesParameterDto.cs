using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.Currency.Dtos
{
    public class ListCurrenciesParameterDto : FullAuditedEntityDto
    {
        public bool IsActive { get; set; } = true;
    }
}
