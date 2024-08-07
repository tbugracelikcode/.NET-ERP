using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;
using Tsi.Core.Utilities.SqlDataTypeMappingUtilities;
using TSI.QueryBuilder.MappingAttributes;
using TsiErp.Entities.Entities.ProductionManagement.TemplateOperationLine.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.TemplateOperationUnsuitabilityItem.Dtos;

namespace TsiErp.Entities.Entities.ProductionManagement.TemplateOperation.Dtos
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
        /// Yüksek Tamir Maliyeti
        /// </summary>
        public bool IsHighRepairCost { get; set; }
        /// <summary>
        /// Hassas
        /// </summary>
        public bool IsSensitive { get; set; }
        /// <summary>
        /// Fiziksel Zor
        /// </summary>
        public bool IsPhysicallyHard { get; set; }
        /// <summary>
        /// Bilgi Gereklilik
        /// </summary>
        public bool IsRequiresKnowledge { get; set; }
        /// <summary>
        /// Yetenek Gereklilik
        /// </summary>
        public bool IsRequiresSkill { get; set; }
        /// <summary>
        /// Operatöre Riskli
        /// </summary>
        public bool IsRiskyforOperator { get; set; }
        /// <summary>
        /// Operatör Uzun Çalışma Süresi
        /// </summary>
        public bool IsLongWorktimeforOperator { get; set; }
        /// <summary>
        /// Tespit Edilebilir 
        /// </summary>
        public bool IsCanBeDetected { get; set; }
        /// <summary>
        /// Ekstra Maaliyet
        /// </summary>
        public bool IsCauseExtraCost { get; set; }
        /// <summary>
        /// İş Puanı
        /// </summary>
        public int WorkScore { get; set; }

        [NoDatabaseAction]
        public List<SelectTemplateOperationLinesDto> SelectTemplateOperationLines { get; set; }

        [NoDatabaseAction]
        public List<SelectTemplateOperationUnsuitabilityItemsDto> SelectTemplateOperationUnsuitabilityItems { get; set; }

    }
}
