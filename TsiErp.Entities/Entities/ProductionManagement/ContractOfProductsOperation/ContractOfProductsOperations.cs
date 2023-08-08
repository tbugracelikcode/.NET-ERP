using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;
using Tsi.Core.Utilities.SqlDataTypeMappingUtilities;

namespace TsiErp.Entities.Entities.ProductionManagement.ContractOfProductsOperation
{
    /// <summary>
    /// Ürün Operasyon Fason Tedarikçiler
    /// </summary>
    public class ContractOfProductsOperations : FullAuditedEntity
    {
        [SqlColumnType(Nullable = true, SqlDbType = SqlDataType.UniqueIdentifier)]
        ///<summary>
        ///Operasyon ID
        /// </summary
        public Guid ProductsOperationID { get; set; }

        [SqlColumnType(Nullable = true, SqlDbType = SqlDataType.UniqueIdentifier)]
        ///<summary>
        ///Cari Hesap Kartı ID
        /// </summary
        public Guid CurrentAccountCardID { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.Int)]
        /// <summary>
        /// Satır No
        /// </summary>
        public int LineNr { get; set; }
    }
}
