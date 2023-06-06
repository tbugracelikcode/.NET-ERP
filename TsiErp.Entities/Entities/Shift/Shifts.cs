using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;
using TsiErp.Entities.Entities.CalendarLine;
using TsiErp.Entities.Entities.ContractProductionTracking;
using TsiErp.Entities.Entities.ProductionTracking;
using TsiErp.Entities.Entities.ShiftLine;
using Tsi.Core.Utilities.SqlDataTypeMappingUtilities;
using SqlDataType = Tsi.Core.Utilities.SqlDataTypeMappingUtilities.SqlDataType;

namespace TsiErp.Entities.Entities.Shift
{
    /// <summary>
    /// Vardiyalar
    /// </summary>
    public class Shifts : FullAuditedEntity
    {
        /// <summary>
        /// Vardiya Kodu
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Vardiya Açıklaması
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Aktiflik
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Toplam Çalışma Süresi
        /// </summary>
        public decimal TotalWorkTime { get; set; }

        /// <summary>
        /// Toplam Mola Süresi
        /// </summary>
        public decimal TotalBreakTime { get; set; }

        /// <summary>
        /// Net Çalışma Süresi
        /// </summary>
        public decimal NetWorkTime { get; set; }

        /// <summary>
        /// Fazla Mesai Süresi
        /// </summary>
        public decimal Overtime { get; set; }
        /// <summary>
        /// Vardiya Sırası
        /// </summary>
        public int ShiftOrder { get; set; }

    }
}
