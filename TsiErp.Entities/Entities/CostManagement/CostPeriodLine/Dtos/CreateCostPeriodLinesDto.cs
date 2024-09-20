using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.CostManagement.CostPeriodLine.Dtos
{
    public class CreateCostPeriodLinesDto : FullAuditedEntityDto
    {
        /// <summary>
        /// Maliyet Periyodu ID
        /// </summary>
        public Guid CostPeriodID { get; set; }
        /// <summary>
        /// Satır No
        /// </summary>
        public int LineNr { get; set; }
        /// <summary>
        /// Başlık
        /// </summary>
        public string Title_ { get; set; }
        /// <summary>
        /// Tutar
        /// </summary>
        public decimal Amount { get; set; }
    }
}
