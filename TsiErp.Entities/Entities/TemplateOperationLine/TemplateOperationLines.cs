using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;
using TsiErp.Entities.Entities.Station;
using TsiErp.Entities.Entities.TemplateOperation;

namespace TsiErp.Entities.Entities.TemplateOperationLine
{
    /// <summary>
    /// Operasyon Satırları
    /// </summary>
    public class TemplateOperationLines : FullAuditedEntity
    {
        
        /// <summary>
        /// Şablon Operasyon ID
        /// </summary>
        public Guid TemplateOperationID { get; set; }
        /// <summary>
        /// İstasyon ID
        /// </summary>
        public Guid StationID { get; set; }
        /// <summary>
        /// Öncelik
        /// </summary>
        public int Priority { get; set; }
        /// <summary>
        /// İşlem Adet
        /// </summary>
        public int ProcessQuantity { get; set; }
        /// <summary>
        /// Ayar ve Kontrol Süresi
        /// </summary>
        public int AdjustmentAndControlTime { get; set; }
        /// <summary>
        /// Operasyon Süresi
        /// </summary>
        public decimal OperationTime { get; set; }
        /// <summary>
        /// Satır Numarası
        /// </summary>
        public int LineNr { get; set; }
        /// <summary>
        /// Alternatif
        /// </summary>
        public bool Alternative { get; set; }

        public TemplateOperations TemplateOperations { get; set; }

        public Stations Stations { get; set; }
    }
}
