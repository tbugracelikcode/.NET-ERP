using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.ProductionManagement.ContractOfProductsOperation.Dtos
{
    public class UpdateContractOfProductsOperationsDto : FullAuditedEntityDto
    {
        ///<summary>
        ///Operasyon ID
        /// </summary
        public Guid ProductsOperationID { get; set; }
        ///<summary>
        ///Cari Hesap Kartı ID
        /// </summary
        public Guid CurrentAccountCardID { get; set; }
        /// <summary>
        /// Satır No
        /// </summary>
        public int LineNr { get; set; }
    }
}
