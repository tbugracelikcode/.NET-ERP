using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;
using TsiErp.Entities.Entities.ShiftLine.Dtos;

namespace TsiErp.Entities.Entities.Shift.Dtos
{
    public class SelectShiftsDto : FullAuditedEntityDto
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
        /// Data Concurrency Stamp
        /// </summary>
        public bool? DataOpenStatus { get; set; }

        /// <summary>
        /// Data Concurrency UserID
        /// </summary>
        public Guid? DataOpenStatusUserId { get; set; }
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

        public List<SelectShiftLinesDto> SelectShiftLinesDto { get; set; }
    }
}
