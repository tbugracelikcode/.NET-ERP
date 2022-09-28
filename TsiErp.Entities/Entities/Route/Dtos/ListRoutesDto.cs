using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.Route.Dtos
{
    public class ListRoutesDto : FullAuditedEntityDto
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
        /// Ana Ürün Kodu
        /// </summary>
        public string ProductCode { get; set; }
        /// <summary>
        /// Üretim Başlangıç
        /// </summary>
        public string ProductionStart { get; set; }
        /// <summary>
        /// Onay
        /// </summary>
        public bool Approval { get; set; }
        /// <summary>
        /// Teknik Onay
        /// </summary>
        public bool TechnicalApproval { get; set; }
        /// <summary>
        /// Aktiflik
        /// </summary>
        public bool IsActive { get; set; }
    }
}
