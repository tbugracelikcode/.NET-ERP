using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;
using TsiErp.Entities.Entities.BillsofMaterialLine.Dtos;

namespace TsiErp.Entities.Entities.BillsofMaterial.Dtos
{
    public class SelectBillsofMaterialsDto : FullAuditedEntityDto
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
        /// Rota ID
        /// </summary>
        public Guid? RouteID { get; set; }
        /// <summary>
        /// Genel Açıklama
        /// </summary>
        public string _Description { get; set; }
        /// <summary>
        /// Aktiflik
        /// </summary>
        public bool IsActive { get; set; }
        /// <summary>
        /// Data Concurrency Stamp
        /// </summary>
        public bool? DataOpenStatus { get; set; }

        /// <summary>
        /// Data Concurrency UserID
        /// </summary>
        public Guid? DataOpenStatusUserId { get; set; }

        public List<SelectBillsofMaterialLinesDto> SelectBillsofMaterialLines { get; set; }
    }
}
