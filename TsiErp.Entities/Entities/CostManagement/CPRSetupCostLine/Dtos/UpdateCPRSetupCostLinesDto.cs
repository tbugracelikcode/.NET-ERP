using Tsi.Core.Entities.Auditing;
using TsiErp.Entities.Enums;

namespace TsiErp.Entities.Entities.CostManagement.CPRSetupCostLine.Dtos
{
    public class UpdateCPRSetupCostLinesDto : FullAuditedEntityDto
    {
        /// <summary>
        /// CPR ID
        /// </summary>
        public Guid CPRID { get; set; }
        /// <summary>
        /// Satır No
        /// </summary>
        public int LineNr { get; set; }
        /// <summary>
        /// Üretim Adımları
        /// </summary>
        public int ManufacturingSteps { get; set; }
        /// <summary>
        /// Üretim Pay Boyutu
        /// </summary>
        public int ManufacturingLotSize { get; set; }
        /// <summary>
        /// Kurulum Süresi
        /// </summary>
        public decimal SetupTime { get; set; }
        /// <summary>
        /// Saatlik Kurulum İşçiliği Ücreti
        /// </summary>
        public decimal SetupLaborHourlyRate { get; set; }
        /// <summary>
        /// Saatlik Çalışma Sistemi Ücreti
        /// </summary>
        public decimal WorkingSystemHourlyRate { get; set; }
        /// <summary>
        /// Kurulum Maliyeti
        /// </summary>
        public decimal SetupCost { get; set; }
        /// <summary>
        /// Kalan Üretim Genel Giderleri
        /// </summary>
        public decimal ResidualManufacturingOverhead { get; set; }
        /// <summary>
        /// Birim Kurulum Maliyeti
        /// </summary>
        public decimal UnitSetupCost { get; set; }
    }
}
