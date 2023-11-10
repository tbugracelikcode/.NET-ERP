using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;
using Tsi.Core.Utilities.SqlDataTypeMappingUtilities;

namespace TsiErp.Entities.Entities.ProductionManagement.ContractTrackingFicheAmountEntryLine.Dtos
{
    public class UpdateContractTrackingFicheAmountEntryLinesDto : FullAuditedEntityDto
    {

        /// <summary>
        /// Fason Takip Fişi ID
        /// </summary>
        public Guid ContractTrackingFicheID { get; set; }
        /// <summary>
        /// Satır no
        /// </summary>
        public int LineNr { get; set; }
        /// <summary>
        /// Tarih
        /// </summary>
        public DateTime? Date_ { get; set; }
        /// <summary>
        /// Adet
        /// </summary>
        public int Amount_ { get; set; }
        /// <summary>
        /// Adet
        /// </summary>
        public string Description_ { get; set; }
    }
}
