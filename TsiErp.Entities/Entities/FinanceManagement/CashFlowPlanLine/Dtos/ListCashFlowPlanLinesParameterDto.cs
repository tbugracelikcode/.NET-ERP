using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.FinanceManagement.CashFlowPlanLine.Dtos
{
    public class ListCashFlowPlanLinesParameterDto : FullAuditedEntityDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid CashFlowPlanID { get; set; }
    }
}
