using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.ProductionManagement.ProductsOperationLine.Dtos
{
    public class CreateProductsOperationLinesDto : FullAuditedEntityDto
    {
        /// <summary>
        /// Ürüne Özel Operasyon ID
        /// </summary>
        public Guid? ProductsOperationID { get; set; }
        /// <summary>
        /// İstasyon ID
        /// </summary>
        public Guid? StationID { get; set; }
        /// <summary>
        /// Öncelik
        /// </summary>
        public int Priority { get; set; }
        /// <summary>
        /// İşlem Adet
        /// </summary>
        public int ProcessQuantity { get; set; }
        /// <summary>
        /// Operasyon Süresi
        /// </summary>
        public decimal OperationTime { get; set; }
        /// <summary>
        /// Ayar ve Kontrol Süresi
        /// </summary>
        public decimal AdjustmentAndControlTime { get; set; }
        /// <summary>
        /// Satır Numarası
        /// </summary>
        public int LineNr { get; set; }
        /// <summary>
        /// Alternatif
        /// </summary>
        public bool Alternative { get; set; }
    }
}
