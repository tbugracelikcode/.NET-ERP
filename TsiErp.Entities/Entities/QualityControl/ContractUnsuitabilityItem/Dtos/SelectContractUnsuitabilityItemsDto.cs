using System;
using System.Collections.Generic;
using System.Text;
using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.QualityControl.ContractUnsuitabilityItem.Dtos
{
    public class SelectContractUnsuitabilityItemsDto : FullAuditedEntityDto
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
        /// Data Concurrency Stamp
        /// </summary>
        public bool? DataOpenStatus { get; set; }

        /// <summary>
        /// Data Concurrency UserID
        /// </summary>
        public Guid? DataOpenStatusUserId { get; set; }
        /// <summary>
        /// Aktiflik
        /// </summary>
        public bool IsActive { get; set; }
    }
}
