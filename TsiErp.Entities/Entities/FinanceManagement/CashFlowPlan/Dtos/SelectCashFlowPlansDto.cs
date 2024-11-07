using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities;
using Tsi.Core.Entities.Auditing;
using TSI.QueryBuilder.MappingAttributes;
using TsiErp.Entities.Entities.FinanceManagement.CashFlowPlanLine.Dtos;

namespace TsiErp.Entities.Entities.FinanceManagement.CashFlowPlan.Dtos
{
    public class SelectCashFlowPlansDto : FullAuditedEntityDto
    {
        /// <summary>
        /// Kod
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Başlangıç Tarihi
        /// </summary>
        public DateTime StartDate { get; set; }
        /// <summary>
        /// Bitiş Tarihi
        /// </summary>
        public DateTime EndDate { get; set; }
        /// <summary>
        /// Açıklama
        /// </summary>
        public string Description_ { get; set; }
        /// <summary>
        /// Aktif
        /// </summary>
        public bool IsActive { get; set; }

        [NoDatabaseAction]
        public List<SelectCashFlowPlanLinesDto> SelectCashFlowPlanLines { get; set; }
    }
}
