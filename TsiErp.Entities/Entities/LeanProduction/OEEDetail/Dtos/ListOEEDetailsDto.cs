using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities;
using Tsi.Core.Utilities.SqlDataTypeMappingUtilities;

namespace TsiErp.Entities.Entities.LeanProduction.OEEDetail.Dtos
{
    public class ListOEEDetailsDto : IEntityDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Tarih
        /// </summary>
        public DateTime Date_ { get; set; }
        /// <summary>
        /// Ay
        /// </summary>
        public int Month_ { get; set; }
        /// <summary>
        /// Yıl
        /// </summary>
        public int Year_ { get; set; }
        /// <summary>
        /// İstasyon ID
        /// </summary>
        public Guid StationID { get; set; }
        /// <summary>
        /// Personel ID
        /// </summary>
        public Guid EmployeeID { get; set; }
        /// <summary>
        /// İş Emri ID
        /// </summary>
        public Guid WorkOrderID { get; set; }
        /// <summary>
        /// Planlanan Miktar
        /// </summary>
        public decimal PlannedQuantity { get; set; }
        /// <summary>
        /// Üretilen Miktar
        /// </summary>
        public decimal ProducedQuantity { get; set; }
        /// <summary>
        /// Hurda Miktar
        /// </summary>
        public decimal ScrapQuantity { get; set; }
        /// <summary>
        /// Planlanan Süre
        /// </summary>
        public decimal PlannedTime { get; set; }
        /// <summary>
        /// Gerçekleşen Süre
        /// </summary>
        public decimal OccuredTime { get; set; }
    }
}
