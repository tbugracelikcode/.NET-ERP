using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.QualityControl.ProductionOrderChangeItem.Dtos
{
    public class ListProductionOrderChangeItemsDto : FullAuditedEntityDto
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
        /// Tespit
        /// </summary>
        public int Detection { get; set; }
        /// <summary>
        /// Şiddet
        /// </summary>
        public int Severity { get; set; }
        /// <summary>
        /// İstasyon Verimlilik Analizi
        /// </summary>
        public bool StaProductivityAnalysis { get; set; }
        /// <summary>
        /// Personel Verimlilik Analizi
        /// </summary>
        public bool PerProductivityAnalysis { get; set; }
        /// <summary>
        /// Aktiflik
        /// </summary>
        public bool IsActive { get; set; }
    }
}
