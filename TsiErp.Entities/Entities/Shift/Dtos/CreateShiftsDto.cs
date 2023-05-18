using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;
using TSI.QueryBuilder.MappingAttributes;
using TsiErp.Entities.Entities.ShiftLine.Dtos;

namespace TsiErp.Entities.Entities.Shift.Dtos
{
    public class CreateShiftsDto : FullAuditedEntityDto
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
        [Precision(18, 6)]

        /// <summary>
        /// Toplam Çalışma Süresi
        /// </summary>
        public decimal TotalWorkTime { get; set; }
        [Precision(18, 6)]

        /// <summary>
        /// Toplam Mola Süresi
        /// </summary>
        public decimal TotalBreakTime { get; set; }
        [Precision(18, 6)]

        /// <summary>
        /// Net Çalışma Süresi
        /// </summary>
        public decimal NetWorkTime { get; set; }
        [Precision(18, 6)]

        /// <summary>
        /// Fazla Mesai Süresi
        /// </summary>
        public decimal Overtime { get; set; }
        /// <summary>
        /// Vardiya Sırası
        /// </summary>
        public int ShiftOrder { get; set; }
        [NoDatabaseAction]
        public List<SelectShiftLinesDto> SelectShiftLinesDto { get; set; }
    }
}
