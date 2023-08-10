using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.ProductionManagement.ContractOfProductsOperation.Dtos
{
    public class ListContractOfProductsOperationsDto : FullAuditedEntityDto
    {
        /// <summary>
        /// Cari Ünvan
        /// </summary>
        public string Name { get; set; }
    }
}
