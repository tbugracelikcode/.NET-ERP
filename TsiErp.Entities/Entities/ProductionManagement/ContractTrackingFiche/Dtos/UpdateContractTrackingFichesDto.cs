using Tsi.Core.Entities.Auditing;
using TSI.QueryBuilder.MappingAttributes;
using TsiErp.Entities.Entities.ProductionManagement.ContractTrackingFicheAmountEntryLine.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.ContractTrackingFicheLine.Dtos;

namespace TsiErp.Entities.Entities.ProductionManagement.ContractTrackingFiche.Dtos
{
    public class UpdateContractTrackingFichesDto : FullAuditedEntityDto
    {
        /// <summary>
        /// Fiş No
        /// </summary>
        public string FicheNr { get; set; }
        /// <summary>
        /// Fiş Tarihi
        /// </summary>
        public DateTime FicheDate_ { get; set; }
        /// <summary>
        /// Açıklama
        /// </summary>
        public string Description_ { get; set; }
        /// <summary>
        /// Üretim Emri ID
        /// </summary>
        public Guid? ProductionOrderID { get; set; }
        /// <summary>
        /// Cari Hesap ID
        /// </summary>
        public Guid? CurrentAccountCardID { get; set; }
        /// <summary>
        /// Stok ID
        /// </summary>
        public Guid? ProductID { get; set; }
        /// <summary>
        /// Kalite Planı Cari Hesap ID
        /// </summary>
        public Guid QualityPlanCurrentAccountCardID { get; set; }
        /// <summary>
        /// İş Tanımı ID
        /// </summary>
        public Guid? ContractQualityPlanID { get; set; }
        /// <summary>
        /// Adet
        /// </summary>
        public int Amount_ { get; set; }
        /// <summary>
        /// Gerçekleşen Adet
        /// </summary>
        public int OccuredAmount_ { get; set; }
        /// <summary>
        /// Tahmini Geliş Tarihi
        /// </summary>
        public DateTime EstimatedDate_ { get; set; }

        [NoDatabaseAction]
        public List<SelectContractTrackingFicheLinesDto> SelectContractTrackingFicheLines { get; set; }

        [NoDatabaseAction]
        public List<SelectContractTrackingFicheAmountEntryLinesDto> SelectContractTrackingFicheAmountEntryLines { get; set; }
    }
}
