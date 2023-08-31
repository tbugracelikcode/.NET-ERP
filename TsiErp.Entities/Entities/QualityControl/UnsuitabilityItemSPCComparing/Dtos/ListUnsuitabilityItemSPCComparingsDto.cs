using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;
using Tsi.Core.Utilities.SqlDataTypeMappingUtilities;

namespace TsiErp.Entities.Entities.QualityControl.UnsuitabilityItemSPCComparing.Dtos
{
    public class ListUnsuitabilityItemSPCComparingsDto : FullAuditedEntityDto
    {
        /// <summary>
        /// Uygunsuzluk Başlık Adı
        /// </summary>
        public string UnsuitabilityItemName { get; set; }
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
