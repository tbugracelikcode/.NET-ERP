using Tsi.Core.Entities.Auditing;
using Tsi.Core.Utilities.SqlDataTypeMappingUtilities;
using SqlDataType = Tsi.Core.Utilities.SqlDataTypeMappingUtilities.SqlDataType;

namespace TsiErp.Entities.Entities.GeneralSystemIdentifications.PurchaseManagementParameter
{
    public class PurchaseManagementParameters 
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
    }
}
