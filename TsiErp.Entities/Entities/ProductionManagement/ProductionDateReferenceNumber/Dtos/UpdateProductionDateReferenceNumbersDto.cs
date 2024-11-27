using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;
using Tsi.Core.Utilities.SqlDataTypeMappingUtilities;

namespace TsiErp.Entities.Entities.ProductionManagement.ProductionDateReferenceNumber.Dtos
{
    public class UpdateProductionDateReferenceNumbersDto : FullAuditedEntityDto
    {
       
        /// <summary>
        /// Üretim Tarihi Referans No
        /// </summary>
        public string ProductionDateReferenceNo { get; set; }
        /// <summary>
        /// Onay
        /// </summary>
        public bool Confirmation { get; set; }
        /// <summary>
        /// Açıklama
        /// </summary>
        public string _Description { get; set; }
    }
}
