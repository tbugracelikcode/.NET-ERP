using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;
using Tsi.Core.Utilities.SqlDataTypeMappingUtilities;

namespace TsiErp.Entities.Entities.ProductionManagement.OperationStockMovement
{
    public class OperationStockMovements
    {

        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }

        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        ///  Operasyon ID
        /// </summary>
        public Guid OperationID { get; set; }

        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        ///  İş Emri ID
        /// </summary>
        public Guid WorkorderID { get; set; }

        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        ///  Üretim Emri ID
        /// </summary>
        public Guid ProductionorderID { get; set; }

        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        ///  Sipariş ID
        /// </summary>
        public Guid OrderID { get; set; }

        [SqlColumnType(SqlDbType = SqlDataType.Decimal, Precision = 6, Scale = 18)]
        /// <summary>
        ///  Toplam Miktar
        /// </summary>
        public decimal TotalAmount { get; set; }
    }
}
