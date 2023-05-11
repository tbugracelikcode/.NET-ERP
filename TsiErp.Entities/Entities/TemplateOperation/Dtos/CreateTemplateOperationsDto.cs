using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;
using TSI.QueryBuilder.MappingAttributes;
using TsiErp.Entities.Entities.TemplateOperationLine.Dtos;

namespace TsiErp.Entities.Entities.TemplateOperation.Dtos
{
    public class CreateTemplateOperationsDto : FullAuditedEntityDto
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
        /// İş Merkezi ID
        /// </summary>
        public Guid? WorkCenterID { get; set; }
        /// <summary>
        /// Aktiflik
        /// </summary>
        public bool IsActive { get; set; }
        [NoDatabaseAction]
        public List<SelectTemplateOperationLinesDto> SelectTemplateOperationLines { get; set; }

    }
}
