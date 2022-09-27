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
        /// Rota 
        /// </summary>
        public string Route { get; set; }
        /// <summary>
        /// Ana Ürün 
        /// </summary>
        public string Product { get; set; }
        /// <summary>
        /// Operasyon
        /// </summary>
        public string Operation { get; set; }
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
        public string AdjustmentAndControlTime { get; set; }
        /// <summary>
        /// Operasyon Süresi
        /// </summary>
        public string OperationTime { get; set; }
        /// <summary>
        /// Öncelik
        /// </summary>
        public string Priority { get; set; }
        /// <summary>
        /// Satır Numarası
        /// </summary>
        public string LineNr { get; set; }
        /// <summary>
        /// Operasyon Resmi
        /// </summary>
        public byte[] OperationPicture { get; set; }
    }
}
