using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;
using Tsi.Core.Utilities.SqlDataTypeMappingUtilities;

namespace TsiErp.Entities.Entities.MaintenanceManagement.MaintenanceMRP
{
    public class MaintenanceMRPs : FullAuditedEntity
    {
        [SqlColumnType(MaxLength = 17, Nullable = false, SqlDbType = SqlDataType.NVarChar)]
        /// <summary>
        /// Bakım MRP Kodu
        /// </summary>
        public string Code { get; set; }
        [SqlColumnType( Nullable = true, SqlDbType = SqlDataType.DateTime)]
        /// <summary>
        /// Eklenme Tarihi
        /// </summary>
        public DateTime? Date_ { get; set; }
        [SqlColumnType(Nullable = true, SqlDbType = SqlDataType.DateTime)]
        /// <summary>
        /// Filtre Başlangıç Tarihi
        /// </summary>
        public DateTime? FilterStartDate { get; set; }
        [SqlColumnType(Nullable = true, SqlDbType = SqlDataType.DateTime)]
        /// <summary>
        /// Filtre Bitiş Tarihi
        /// </summary>
        public DateTime? FilterEndDate { get; set; }
        [SqlColumnType(Nullable = true, SqlDbType = SqlDataType.Int)]
        /// <summary>
        /// Bakıma Kalan Süre
        /// </summary>
        public int TimeLeftforMaintenance { get; set; }
        [SqlColumnType( Nullable = true, SqlDbType = SqlDataType.NVarCharMax)]
        /// <summary>
        /// Açıklama
        /// </summary>
        public string Description_ { get; set; }
        [SqlColumnType(Nullable = true, SqlDbType = SqlDataType.Bit)]
        /// <summary>
        /// Satırları Birleştir
        /// </summary>
        public bool IsMergeLines { get; set; }
    }
}
