using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.FinanceManagement.PaymentPlan.Dtos
{
    public class ListPaymentPlansParameterDto : FullAuditedEntityDto
    {
        /// <summary>
        /// Aktiflik
        /// </summary>
        public bool IsActive { get; set; } = true;
    }
}
