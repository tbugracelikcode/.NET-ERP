using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities;
using Tsi.Core.Utilities.SqlDataTypeMappingUtilities;
using SqlDataType = Tsi.Core.Utilities.SqlDataTypeMappingUtilities.SqlDataType;

namespace TsiErp.Entities.Entities.Logging
{
    public class Logs : IEntity
    {
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.UniqueIdentifier)]
        public Guid Id { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.DateTime)]
        public DateTime Date_ { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.NVarCharMax)]
        public string MethodName_ { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.Variant)]
        public object BeforeValues { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.Variant)]
        public object AfterValues { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.Variant)]
        public object DiffValues { get; set; }
        [SqlColumnType(MaxLength = 50, Nullable = false, SqlDbType = SqlDataType.NVarChar)]
        public string LogLevel_ { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.UniqueIdentifier)]
        public Guid UserId { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.UniqueIdentifier)]
        public Guid RecordId { get; set; }
    }
}
