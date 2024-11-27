using Tsi.Core.Entities.Auditing;
using TSI.QueryBuilder.MappingAttributes;
using TsiErp.Entities.Entities.PlanningManagement.ShipmentPlanningLine.Dtos;

namespace TsiErp.Entities.Entities.PlanningManagement.ShipmentPlanning.Dtos
{
    public class UpdateShipmentPlanningsDto : FullAuditedEntityDto
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
        /// Üretim Tarihi Referans ID
        /// </summary>
        public Guid? ProductionDateReferenceID { get; set; }
        /// <summary>
        /// Planlanan Yükleme Tarihi
        /// </summary>
        /// 
        public DateTime? PlannedLoadingTime { get; set; }

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


        [NoDatabaseAction]
        public List<SelectShipmentPlanningLinesDto> SelectShipmentPlanningLines { get; set; }
    }
}
