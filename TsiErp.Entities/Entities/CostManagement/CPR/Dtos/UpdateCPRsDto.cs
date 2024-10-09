using Tsi.Core.Entities.Auditing;
using TSI.QueryBuilder.MappingAttributes;
using TsiErp.Entities.Entities.CostManagement.CPRManufacturingCostLine.Dtos;
using TsiErp.Entities.Entities.CostManagement.CPRMaterialCostLine.Dtos;
using TsiErp.Entities.Entities.CostManagement.CPRSetupCostLine.Dtos;
using TsiErp.Entities.Enums;

namespace TsiErp.Entities.Entities.CostManagement.CPR.Dtos
{
    public class UpdateCPRsDto : FullAuditedEntityDto
    {
        /// <summary>
        /// CPR Kod
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Part No
        /// </summary>
        public string PartNo { get; set; }
        /// <summary>
        /// Stok ID
        /// </summary>
        public Guid? ProductID { get; set; }
        /// <summary>
        /// Tepe Hacim
        /// </summary>
        public decimal PeakVolume { get; set; }
        /// <summary>
        /// Incoterms
        /// </summary>
        public CPRsIncotermEnum Incoterms { get; set; }
        /// <summary>
        /// Alıcı ID
        /// </summary>
        public Guid? RecieverID { get; set; }
        /// <summary>
        /// Tedarikçi ID
        /// </summary>
        public Guid? SupplierID { get; set; }
        /// <summary>
        /// Üretim Yeri
        /// </summary>
        public string ManufacturingLocation { get; set; }
        /// <summary>
        /// Yıllık Üretim Saati
        /// </summary>
        public int ProductionHoursperYear { get; set; }
        /// <summary>
        /// Para Birimi ID
        /// </summary>
        public Guid? CurrencyID { get; set; }
        /// <summary>
        /// Miktar
        /// </summary>
        public int Quantity { get; set; }
        /// <summary>
        /// Tarih
        /// </summary>
        public DateTime Date_ { get; set; }
        /// <summary>
        /// Tedarikçi Yetkili
        /// </summary>
        public string SupplierContact { get; set; }
        /// <summary>
        /// Schaeffler Yetkili
        /// </summary>
        public string RecieverContact { get; set; }
        /// <summary>
        /// Fiyat Azaltma Adımları
        /// </summary>
        public string PriceReductionSteps { get; set; }

        [NoDatabaseAction]
        public List<SelectCPRManufacturingCostLinesDto> SelectCPRManufacturingCostLines { get; set; }

        [NoDatabaseAction]
        public List<SelectCPRMaterialCostLinesDto> SelectCPRMaterialCostLines { get; set; }

        [NoDatabaseAction]
        public List<SelectCPRSetupCostLinesDto> SelectCPRSetupCostLines { get; set; }
    }
}
