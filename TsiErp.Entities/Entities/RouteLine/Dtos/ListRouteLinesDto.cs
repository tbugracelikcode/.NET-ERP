using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.RouteLine.Dtos
{
    public class ListRouteLinesDto : FullAuditedEntityDto
    {
        /// <summary>
        /// Kod
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Açıklama
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Rota Kodu
        /// </summary>
        public string RouteCode { get; set; }
        /// <summary>
        /// Ana Ürün Kodu
        /// </summary>
        public string ProductCode { get; set; }
        /// <summary>
        /// Operasyon Adı
        /// </summary>
        public string OperationName { get; set; }
        /// <summary>
        /// Üretim Havuzu 
        /// </summary>
        public string ProductionPool { get; set; }
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
        public int OperationTime { get; set; }
        /// <summary>
        /// Öncelik
        /// </summary>
        public int Priority { get; set; }
        /// <summary>
        /// Satır Numarası
        /// </summary>
        public int LineNr { get; set; }
        /// <summary>
        /// Operasyon Resmi
        /// </summary>
        public byte[] OperationPicture { get; set; }
    }
}
