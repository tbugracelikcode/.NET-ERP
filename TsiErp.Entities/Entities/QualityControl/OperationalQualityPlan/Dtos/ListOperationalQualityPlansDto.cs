using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.QualityControl.OperationalQualityPlan.Dtos
{
    public class ListOperationalQualityPlansDto : FullAuditedEntityDto
    {

        /// <summary>
        /// Ürün Kodu
        /// </summary>
        public string ProductCode { get; set; }

        /// <summary>
        /// Operasyon Kodu
        /// </summary>
        public string OperationCode { get; set; }
    }
}
