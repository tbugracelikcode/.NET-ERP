using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;
using TsiErp.Entities.Entities.CalendarLine.Dtos;

namespace TsiErp.Entities.Entities.Calendar.Dtos
{
    public class SelectCalendarsDto : FullAuditedEntityDto
    {
        /// <summary>
        /// Çalışma Takvimi Kodu
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Çalışma Takvimi Adı
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Açıklama
        /// </summary>
        public string _Description { get; set; }
        /// <summary>
        /// Planlanan mı?
        /// </summary>
        public bool IsPlanned { get; set; }
        /// <summary>
        /// Yıl
        /// </summary>
        public int Year { get; set; }

        public List<SelectCalendarLinesDto> SelectCalendarLinesDto { get; set; }
    }
}
