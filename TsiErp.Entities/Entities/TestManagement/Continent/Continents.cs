using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;
using Tsi.Core.Utilities.SqlDataTypeMappingUtilities;

namespace TsiErp.Entities.Entities.TestManagement.Continent
{
    public class Continents : FullAuditedEntity
    {
        [SqlColumnType(MaxLength = 17, Nullable = false, SqlDbType = SqlDataType.NVarChar)]
        /// <summary>
        /// Kıta Kod
        /// </summary>
        public string Code { get; set; }
        [SqlColumnType(MaxLength = 200, Nullable = false, SqlDbType = SqlDataType.NVarChar)]
        /// <summary>
        /// Kıta Adı
        /// </summary>
        public string Name { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.Int)]
        /// <summary>
        /// Nüfus
        /// </summary>
        public int Population_ { get; set; }
        [SqlColumnType(Nullable = true, SqlDbType = SqlDataType.NVarCharMax)]
        /// <summary>
        /// Açıklama
        /// </summary>
        public string Description_ { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// Çalışan ID
        /// </summary>
        public Guid EmployeeID { get; set; }
    }
}
