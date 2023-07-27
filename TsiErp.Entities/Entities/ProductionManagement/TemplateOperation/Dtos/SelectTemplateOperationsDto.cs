using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;
using TSI.QueryBuilder.MappingAttributes;
using TsiErp.Entities.Entities.ProductionManagement.TemplateOperationLine.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.TemplateOperationUnsuitabilityItem.Dtos;

namespace TsiErp.Entities.Entities.ProductionManagement.TemplateOperation.Dtos
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
        public string WorkCenterName { get; set; }
        /// <summary>
        /// Aktiflik
        /// </summary>
        public bool IsActive { get; set; }

        [NoDatabaseAction]
        public List<SelectTemplateOperationLinesDto> SelectTemplateOperationLines { get; set; }

        [NoDatabaseAction]
        public List<SelectTemplateOperationUnsuitabilityItemsDto> SelectTemplateOperationUnsuitabilityItems { get; set; }
    }
}
