using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;
using Tsi.Core.Utilities.SqlDataTypeMappingUtilities;

namespace TsiErp.Entities.Entities.QualityControl.OperationalSPCComparing.Dtos
{
    public class CreateOperationalSPCComparingsDto : FullAuditedEntityDto
    {
        /// <summary>
        /// Operasyon ID
        /// </summary>
        public Guid? OperationID { get; set; }
        /// <summary>
        /// Stok ID
        /// </summary>
        public Guid? ProductID { get; set; }
        /// <summary>
        /// Tarih 1
        /// </summary>
        public DateTime Date1 { get; set; }
        /// <summary>
        /// Tarih 2
        /// </summary>
        public DateTime Date2 { get; set; }
    }
}
