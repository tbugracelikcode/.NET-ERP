using System;
using System.Collections.Generic;
using System.Text;
using Tsi.Core.Entities.Auditing;
using TsiErp.Entities.Entities.OperationLine.Dtos;

namespace TsiErp.Entities.Entities.Operation.Dtos
{
    public class SelectOperationsDto : FullAuditedEntityDto
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
        /// Aktiflik
        /// </summary>
        public bool IsActive { get; set; }
        /// <summary>
        /// Üretim Havuz ID
        /// </summary>
        public Guid ProductionPoolID { get; set; }

        public List<SelectOperationLinesDto> SelectOperationLines { get; set; }
    }
}
