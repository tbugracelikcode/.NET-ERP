using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.QualityControl.UnsuitabilityTypesItem.Dtos
{
    public class SelectUnsuitabilityTypesItemsDto : FullAuditedEntityDto
    {
        /// <summary>
        /// Başlık Kodu
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Başlık 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Açıklama 
        /// </summary>
        public string Description_ { get; set; }

        public bool IsActive { get; set; }
    }
}
