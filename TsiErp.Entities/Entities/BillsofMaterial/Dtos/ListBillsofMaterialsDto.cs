using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.BillsofMaterial.Dtos
{
    public class ListBillsofMaterialsDto : FullAuditedEntityDto
    {
        /// <summary>
        /// Reçete Kodu
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Reçete Adı
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Mamül ID
        /// </summary>
        public Guid? FinishedProductID { get; set; }

        /// <summary>
        /// Mamül Kodu
        /// </summary>
        public string FinishedProductCode { get; set; }
        /// <summary>
        /// Mamül Açıklaması
        /// </summary>
        public string FinishedProducName { get; set; }

        /// <summary>
        /// Genel Açıklama
        /// </summary>
        public string _Description { get; set; }
        /// <summary>
        /// Aktiflik
        /// </summary>
        public bool IsActive { get; set; }
    }
}
