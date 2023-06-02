using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;
using TsiErp.Entities.Enums;

namespace TsiErp.Entities.Entities.ShiftLine.Dtos
{
    public class SelectShiftLinesDto : FullAuditedEntityDto
    {
        /// <summary>
        /// Vardiya ID
        /// </summary>
        public Guid ShiftID { get; set; }
        /// <summary>
        /// Satır Nr
        /// </summary>
        public int LineNr { get; set; }
        /// <summary>
        /// Başlangıç Saati
        /// </summary>
        public TimeSpan? StartHour { get; set; }
        /// <summary>
        /// Bitiş Saati
        /// </summary>
        public TimeSpan? EndHour { get; set; }
        /// <summary>
        /// Tür Enum
        /// </summary>
        public ShiftLinesTypeEnum Type { get; set; }
        /// <summary>
        /// Tür Enum Adı
        /// </summary>
        public string TypeName { get; set; }
        /// <summary>
        /// Katsayı
        /// </summary>
        public decimal Coefficient { get; set; }
    }
}
