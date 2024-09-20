using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;
using Tsi.Core.Utilities.SqlDataTypeMappingUtilities;

namespace TsiErp.Entities.Entities.CostManagement.StandartStationCostRecord.Dtos
{
    public class ListStandartStationCostRecordsDto : FullAuditedEntityDto
    {
        /// <summary>
        /// İş İstasyonu Kodu
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// İş İstasyonu ID
        /// </summary>
        public Guid? StationID { get; set; }
        /// <summary>
        /// İstasyon Kod
        /// </summary>
        public string StationCode { get; set; }
        /// <summary>
        /// İstasyon Adı
        /// </summary>
        public string StationName { get; set; }
        /// <summary>
        /// Başlangıç Tarihi
        /// </summary>
        public DateTime? StartDate { get; set; }
        /// <summary>
        /// Bitiş Tarihi
        /// </summary>
        public DateTime? EndDate { get; set; }
        /// <summary>
        /// Para Birimi ID
        /// </summary>
        public Guid? CurrencyID { get; set; }
        /// <summary>
        /// Para Birimi Kodu
        /// </summary>
        public string CurrencyCode { get; set; }
        /// <summary>
        /// Maliyet
        /// </summary>
        public decimal StationCost { get; set; }
        /// <summary>
        /// Standart
        /// </summary>
        public bool isStandart { get; set; }
    }
}
