using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.ProductionManagement.RouteLine.Dtos
{
    public class ListRouteLinesDto : FullAuditedEntityDto
    {

        /// <summary>
        /// Rota Kodu
        /// </summary>
        public string RouteCode { get; set; }
        /// <summary>
        /// Ana Ürün Kodu
        /// </summary>
        public string ProductCode { get; set; }
        /// <summary>
        /// Ana Ürün Açıklaması
        /// </summary>
        public string ProductName { get; set; }
        /// <summary>
        /// Operasyon Adı
        /// </summary>
        public string OperationName { get; set; }
        /// <summary>
        /// Operasyon Kodu
        /// </summary>
        public string OperationCode { get; set; }
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
