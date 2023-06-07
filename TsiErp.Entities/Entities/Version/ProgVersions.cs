using Tsi.Core.Utilities.SqlDataTypeMappingUtilities;

namespace TsiErp.Entities.Entities.Version
{
    public class ProgVersions
    {
        [SqlColumnType(IsPrimaryKey =true,SqlDbType =SqlDataType.UniqueIdentifier)]
        public Guid Id { get; set; }

        [SqlColumnType(Nullable =false,SqlDbType =SqlDataType.NVarChar,MaxLength =3)]
        public string MajDbVersion { get; set; }

        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.NVarChar, MaxLength = 4)]
        public string MinDbVersion { get; set; }

        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.NVarChar, MaxLength = 5)]
        public string BuildVersion { get; set; }

        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.Bit)]
        public bool IsUpdating { get; set; }
    }
}
