using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;
using TsiErp.Entities.Entities.TemplateOperationLine.Dtos;

namespace TsiErp.Entities.Entities.TemplateOperation.Dtos
{
    public class SelectTemplateOperationsDto : FullAuditedEntityDto
    {
        /// <summary>
        /// Kod
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Açıklama
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
        /// İş Merkezi ID
        /// </summary>
        public Guid? WorkCenterID { get; set; }
        /// <summary>
        /// İş Merkezi Kodu
        /// </summary>
        public string WorkCenterCode { get; set; }
        /// <summary>
        /// Aktiflik
        /// </summary>
        public bool IsActive { get; set; }

        public List<SelectTemplateOperationLinesDto> SelectTemplateOperationLines { get; set; }
    }
}
