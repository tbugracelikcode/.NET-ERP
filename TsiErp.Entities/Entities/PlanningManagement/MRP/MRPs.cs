using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;
using Tsi.Core.Utilities.SqlDataTypeMappingUtilities;
using TsiErp.Entities.Enums;

namespace TsiErp.Entities.Entities.PlanningManagement.MRP
{
    public class MRPs : FullAuditedEntity
    {
        [SqlColumnType(MaxLength = 17, Nullable = false, SqlDbType = SqlDataType.NVarChar)]
        /// <summary>
        /// Kod
        /// </summary>
        public string Code { get; set; }
        [SqlColumnType( Nullable = true, SqlDbType = SqlDataType.NVarCharMax)]
        /// <summary>
        /// Açıklama
        /// </summary>
        public string Description_ { get; set; }
        [SqlColumnType(Nullable = true, SqlDbType = SqlDataType.DateTime)]
        /// <summary>
        /// Tarih
        /// </summary>
        public DateTime Date_ { get; set; }
        [SqlColumnType(Nullable = true, SqlDbType = SqlDataType.Int)]
        /// <summary>
        /// Durum
        /// </summary>
        public MRPsStateEnum State_ { get; set; }
        [SqlColumnType(Nullable = true, SqlDbType = SqlDataType.Bit)]
        /// <summary>
        /// Bakım MRP'den mi
        /// </summary>
        public bool IsMaintenanceMRP { get; set; }
        [SqlColumnType(Nullable = true, SqlDbType = SqlDataType.Bit)]
        /// <summary>
        /// Bakım MRP ID
        /// </summary>
        public Guid? MaintenanceMRPID { get; set; }
        [SqlColumnType(Nullable = true, SqlDbType = SqlDataType.Bit)]
        /// <summary>
        /// Sipariş Kabul ID
        /// </summary>
        public Guid? OrderAcceptanceID { get; set; }
        [SqlColumnType(Nullable = true, SqlDbType = SqlDataType.DateTime)]
        /// <summary>
        /// Referans Tarihi
        /// </summary>
        public DateTime ReferanceDate { get; set; }
    }
}
