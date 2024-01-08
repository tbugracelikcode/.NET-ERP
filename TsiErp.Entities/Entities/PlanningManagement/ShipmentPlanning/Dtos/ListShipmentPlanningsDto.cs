using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.PlanningManagement.ShipmentPlanning.Dtos
{
    public class ListShipmentPlanningsDto : FullAuditedEntityDto
    {
        /// <summary>
        /// Kod
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Açıklama
        /// </summary>
        public string Description_ { get; set; }

        /// <summary>
        /// Sevkiyat Planlama Tarihi
        /// </summary>
        /// 
        public DateTime ShipmentPlanningDate { get; set; }

        /// <summary>
        /// Toplam Net KG
        /// </summary>
        public decimal TotalNetKG { get; set; }

        /// <summary>
        /// Toplam Brüt KG
        /// </summary>
        public decimal TotalGrossKG { get; set; }

        /// <summary>
        /// Toplam Adet
        /// </summary>
        public int TotalAmount { get; set; }

        /// <summary>
        /// Toplam Adet
        /// </summary>
        public int PlannedAmount { get; set; }
    }
}
