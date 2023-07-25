using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;
using TSI.QueryBuilder.MappingAttributes;
using TsiErp.Entities.Entities.ProductionManagement.TemplateOperationLine.Dtos;

namespace TsiErp.Entities.Entities.ProductionManagement.TemplateOperation.Dtos
{
    public class ListTemplateOperationsDto : FullAuditedEntityDto
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
        /// İş Merkezi Kodu
        /// </summary>
        public string WorkCenterName { get; set; }
        /// <summary>
        /// Aktiflik
        /// </summary>
        public bool IsActive { get; set; }

        [NoDatabaseAction]
        public List<SelectTemplateOperationLinesDto> SelectTemplateOperationLines { get; set; }
    }
}
