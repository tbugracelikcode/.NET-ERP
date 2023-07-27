using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.ProductionManagement.TemplateOperationUnsuitabilityItem.Dtos
{
    public class SelectTemplateOperationUnsuitabilityItemsDto : FullAuditedEntityDto
    {

        /// <summary>
        /// Kullanılacak
        /// </summary>
        public bool ToBeUsed { get; set; }

        /// <summary>
        /// Satır Numarası
        /// </summary>
        public int LineNr { get; set; }

        ///<summary>
        ///Uygunsuzluk Başlığı ID
        /// </summary
        public Guid UnsuitabilityItemsId { get; set; }

        /// <summary>
        /// Uygunsuzluk Başlığı Adı
        /// </summary>
        public string UnsuitabilityItemsName { get; set; }
    }
}
