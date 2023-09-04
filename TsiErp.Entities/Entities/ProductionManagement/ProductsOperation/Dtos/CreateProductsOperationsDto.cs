using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;
using TSI.QueryBuilder.MappingAttributes;
using TsiErp.Entities.Entities.ProductionManagement.ContractOfProductsOperation.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.ProductsOperationLine.Dtos;

namespace TsiErp.Entities.Entities.ProductionManagement.ProductsOperation.Dtos
{
    public class CreateProductsOperationsDto : FullAuditedEntityDto
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
        /// Ürün ID
        /// </summary>
        public Guid? ProductID { get; set; }
        /// <summary>
        /// Şablon Operasyon ID
        /// </summary>
        public Guid? TemplateOperationID { get; set; }
        /// <summary>
        /// Aktiflik
        /// </summary>
        public bool IsActive { get; set; }

        [NoDatabaseAction]
        public List<SelectProductsOperationLinesDto> SelectProductsOperationLines { get; set; }


        [NoDatabaseAction]
        public List<SelectContractOfProductsOperationsDto> SelectContractOfProductsOperationsLines { get; set; }
    }
}
