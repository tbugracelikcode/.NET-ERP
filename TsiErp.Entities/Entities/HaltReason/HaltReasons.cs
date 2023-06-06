using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;
using TsiErp.Entities.Entities.ProductionTrackingHaltLine;
using Tsi.Core.Utilities.SqlDataTypeMappingUtilities;
using SqlDataType = Tsi.Core.Utilities.SqlDataTypeMappingUtilities.SqlDataType;

namespace TsiErp.Entities.Entities.HaltReason
{
    public class HaltReasons : FullAuditedEntity
    {
        [SqlColumnType(MaxLength = 200, Nullable = false, SqlDbType = SqlDataType.NVarChar)]
        /// <summary>
        /// Duruş Sebebi Adı
        /// </summary>
        public string Name { get; set; }
        [SqlColumnType(MaxLength = 17, Nullable = false, SqlDbType = SqlDataType.NVarChar)]
        /// <summary>
        /// Duruş Sebebi Kodu
        /// </summary>
        public string Code { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.Bit)]
        /// <summary>
        /// Planlı mı?
        /// </summary>
        public bool IsPlanned { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.Bit)]
        /// <summary>
        /// Makine Kaynaklı mı?
        /// </summary>
        public bool IsMachine { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.Bit)]
        /// <summary>
        /// Operatör Kaynaklı mı?
        /// </summary>
        public bool IsOperator { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.Bit)]
        /// <summary>
        /// Yönetim Kaynaklı mı?
        /// </summary>
        public bool IsManagement { get; set; }
    }
}
