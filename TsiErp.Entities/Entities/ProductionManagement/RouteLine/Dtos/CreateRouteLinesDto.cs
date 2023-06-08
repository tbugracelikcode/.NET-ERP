using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.ProductionManagement.RouteLine.Dtos
{
    public class CreateRouteLinesDto : FullAuditedEntityDto
    {

        /// <summary>
        /// Rota ID
        /// </summary>
        public Guid? RouteID { get; set; }
        /// <summary>
        /// Ana Ürün ID
        /// </summary>
        public Guid? ProductID { get; set; }
        /// <summary>
        /// Operasyon ID
        /// </summary>
        public Guid? ProductsOperationID { get; set; }
        /// <summary>
        /// Üretim Havuz ID
        /// </summary>
        public Guid? ProductionPoolID { get; set; }
        /// <summary>
        /// Üretim Havuz Açıklama
        /// </summary>
        public string ProductionPoolDescription { get; set; }
        /// <summary>
        /// Ayar ve Kontrol Süresi
        /// </summary>
        public int AdjustmentAndControlTime { get; set; }
        /// <summary>
        /// Operasyon Süresi
        /// </summary>
        public decimal OperationTime { get; set; }
        /// <summary>
        /// Öncelik
        /// </summary>
        public int Priority { get; set; }
        /// <summary>
        /// Satır Numarası
        /// </summary>
        public int LineNr { get; set; }
    }
}
