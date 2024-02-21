using Tsi.Core.Entities;
using Tsi.Core.Entities.Auditing;
using Tsi.Core.Utilities.SqlDataTypeMappingUtilities;
using SqlDataType = Tsi.Core.Utilities.SqlDataTypeMappingUtilities.SqlDataType;

namespace TsiErp.Entities.Entities.GeneralSystemIdentifications.PlanningManagementParameter
{
    public class PlanningManagementParameters : IEntity
    {
        [SqlColumnType(Nullable = true, SqlDbType = SqlDataType.Bit)]
        /// <summary>
        /// İleri Zamanlı Tarih Parametresi
        /// </summary>
        public bool FutureDateParameter { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.Int)]
        /// <summary>
        /// MRP Satınalma Sürecinin Başlangıç Parametresi
        /// </summary>
        public int MRPPurchaseTransaction { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.Int)]
        /// <summary>
        /// MRPII Kaynak Modül Parametresi
        /// </summary>
        public int MRPIISourceModule { get; set; }
    }
}
