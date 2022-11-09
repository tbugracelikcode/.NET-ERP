using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;
using TsiErp.Entities.Entities.BillsofMaterialLine.Dtos;

namespace TsiErp.Entities.Entities.BillsofMaterial.Dtos
{
    public class CreateBillsofMaterialsDto : FullAuditedEntityDto
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

        public List<SelectBillsofMaterialLinesDto> SelectBillsofMaterialLines { get; set; }
    }
}
