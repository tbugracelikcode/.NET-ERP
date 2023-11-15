using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.MaintenanceManagement.MaintenanceMRP.Dtos
{
    public class ListMaintenanceMRPsDto : FullAuditedEntityDto
    {
        /// <summary>
        /// Bakım MRP Kodu
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Eklenme Tarihi
        /// </summary>
        public DateTime? Date_ { get; set; }
        /// <summary>
        /// Filtre Başlangıç Tarihi
        /// </summary>
        public DateTime? FilterStartDate { get; set; }
        /// <summary>
        /// Filtre Bitiş Tarihi
        /// </summary>
        public DateTime? FilterEndDate { get; set; }
        /// <summary>
        /// Bakıma Kalan Süre
        /// </summary>
        public int TimeLeftforMaintenance { get; set; }
        /// <summary>
        /// Açıklama
        /// </summary>
        public string Description_ { get; set; }
        /// <summary>
        /// Satırları Birleştir
        /// </summary>
        public bool IsMergeLines { get; set; }

    }
}
