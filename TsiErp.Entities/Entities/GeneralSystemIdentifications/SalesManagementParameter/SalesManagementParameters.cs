using Tsi.Core.Entities;
using Tsi.Core.Utilities.SqlDataTypeMappingUtilities;
using SqlDataType = Tsi.Core.Utilities.SqlDataTypeMappingUtilities.SqlDataType;

namespace TsiErp.Entities.Entities.GeneralSystemIdentifications.SalesManagementParameter
{
    public class SalesManagementParameters : IEntity
    {
        [SqlColumnType(Nullable = true, SqlDbType = SqlDataType.Bit)]
        /// <summary>
        /// Sipariş İleri Zamanlı Tarih Parametresi
        /// </summary>
        public bool OrderFutureDateParameter { get; set; }
        [SqlColumnType(Nullable = true, SqlDbType = SqlDataType.Bit)]
        /// <summary>
        /// Teklif İleri Zamanlı Tarih Parametresi
        /// </summary>
        public bool PropositionFutureDateParameter { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.Int)]
        /// <summary>
        /// Satış Sipariş Kur Türü
        /// </summary>
        public int SalesOrderExchangeRateType { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.Int)]
        /// <summary>
        /// Verilen Teklif Kur Türü
        /// </summary>
        public int SalesPropositionExchangeRateType { get; set; }
    }
}
