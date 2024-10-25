using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;
using Tsi.Core.Utilities.SqlDataTypeMappingUtilities;

namespace TsiErp.Entities.Entities.ProductionManagement.ContractTrackingFicheAmountEntryLine
{
    public class ContractTrackingFicheAmountEntryLines : FullAuditedEntity
    {
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// Fason Takip Fişi ID
        /// </summary>
        public Guid ContractTrackingFicheID { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.Int)]
        /// <summary>
        /// Satır no
        /// </summary>
        public int LineNr { get; set; }
        [SqlColumnType(Nullable = true, SqlDbType = SqlDataType.DateTime)]
        /// <summary>
        /// Tarih
        /// </summary>
        public DateTime? Date_ { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.Int)]
        /// <summary>
        /// Adet
        /// </summary>
        public int Amount_ { get; set; }
        [SqlColumnType(Nullable = true, SqlDbType = SqlDataType.NVarCharMax)]
        /// <summary>
        /// Adet
        /// </summary>
        public string Description_ { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// Cari Hesap ID
        /// </summary>
        public Guid CurrentAccountID { get; set; }
    }
}
