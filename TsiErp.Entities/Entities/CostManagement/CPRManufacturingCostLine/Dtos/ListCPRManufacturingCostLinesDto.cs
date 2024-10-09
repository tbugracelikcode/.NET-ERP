using Tsi.Core.Entities.Auditing;
using TsiErp.Entities.Enums;

namespace TsiErp.Entities.Entities.CostManagement.CPRManufacturingCostLine.Dtos
{
    public class ListCPRManufacturingCostLinesDto : FullAuditedEntityDto
    {
        /// <summary>
        /// CPR ID
        /// </summary>
        public Guid CPRID { get; set; }
        /// <summary>
        /// Operasyon ID
        /// </summary>
        public Guid? ProductsOperationID { get; set; }
        /// <summary>
        /// Operasyon Adı
        /// </summary>
        public string ProductsOperationName { get; set; }
        /// <summary>
        /// Satır No
        /// </summary>
        public int LineNr { get; set; }
        /// <summary>
        /// Üretim Adımları
        /// </summary>
        public CPRsManufacturingStepsEnum ManufacturingSteps { get; set; }
        /// <summary>
        /// Malzeme
        /// </summary>
        public string Material_ { get; set; }
        /// <summary>
        /// İstasyon ID
        /// </summary>
        public Guid? StationID { get; set; }
        /// <summary>
        /// İstasyon Kodu
        /// </summary>
        public string StationCode { get; set; }
        /// <summary>
        /// İstasyon Adı
        /// </summary>
        public string StationName { get; set; }
        /// <summary>
        /// Net Çıkış / OEE
        /// </summary>
        public int NetOutputDVOEE { get; set; }
        /// <summary>
        /// Döngü Başına Parça
        /// </summary>
        public int PartsperCycle { get; set; }
        /// <summary>
        /// Çalışma Sistemi Yatırım
        /// </summary>
        public decimal WorkingSystemInvest { get; set; }
        /// <summary>
        /// Çalışma Sistemi Saatlik Oran
        /// </summary>
        public decimal WorkingSystemHourlyRate { get; set; }
        /// <summary>
        /// Çalışma Sistemi Parça Başına Oran
        /// </summary>
        public decimal WorkingSystemCostperPart { get; set; }
        /// <summary>
        /// Doğrudan İşçilik Saatlik Ücret
        /// </summary>
        public decimal DirectLaborHourlyRate { get; set; }
        /// <summary>
        /// Çalışma Sistemindeki Kişi Sayısı
        /// </summary>
        public int HeadCountatWorkingSystem { get; set; }
        /// <summary>
        /// Parça Başına İşçilik Maliyeti
        /// </summary>
        public decimal LaborCostperPart { get; set; }
        /// <summary>
        /// Kalan Üretim Genel Giderleri
        /// </summary>
        public decimal ResidualManufacturingOverhead { get; set; }
        /// <summary>
        /// Hurda Oranı
        /// </summary>
        public decimal ScrapRate { get; set; }
        /// <summary>
        /// Hurda Maliyeti
        /// </summary>
        public decimal ScrapCost { get; set; }
        /// <summary>
        /// Üretim Adımı Maliyeti
        /// </summary>
        public decimal ManufacuringStepCost { get; set; }
        /// <summary>
        /// Fason İmalat
        /// </summary>
        public string ContractProduction { get; set; }
        /// <summary>
        /// Fason Birim Maliyeti
        /// </summary>
        public decimal ContractUnitCost { get; set; }
        /// <summary>
        /// OEE Dahil Edilsin
        /// </summary>
        public string IncludingOEE { get; set; }
    }
}
