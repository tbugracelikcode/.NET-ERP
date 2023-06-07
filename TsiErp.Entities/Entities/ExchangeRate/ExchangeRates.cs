using Tsi.Core.Entities.Auditing;
using Tsi.Core.Utilities.SqlDataTypeMappingUtilities;
using SqlDataType = Tsi.Core.Utilities.SqlDataTypeMappingUtilities.SqlDataType;

namespace TsiErp.Entities.Entities.ExchangeRate
{
    /// <summary>
    /// Kurlar
    /// </summary>
    public class ExchangeRates : FullAuditedEntity
    {
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// Parabirimi ID
        /// </summary>
        public Guid CurrencyID { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.DateTime)]
        public DateTime Date { get; set; }

        [SqlColumnType(SqlDbType = SqlDataType.Decimal, Precision = 6, Scale = 18)]
        /// <summary>
        /// Alış Kuru
        /// </summary>
        public decimal BuyingRate { get; set; }

        [SqlColumnType(SqlDbType = SqlDataType.Decimal, Precision = 6, Scale = 18)]
        /// <summary>
        /// Satış Kuru
        /// </summary>
        public decimal SaleRate { get; set; }

        [SqlColumnType(SqlDbType = SqlDataType.Decimal, Precision = 6, Scale = 18)]
        /// <summary>
        /// Efektif Alış Kuru
        /// </summary>
        public decimal EffectiveBuyingRate { get; set; }

        [SqlColumnType(SqlDbType = SqlDataType.Decimal, Precision = 6, Scale = 18)]
        /// <summary>
        /// Efektif Satış Kuru
        /// </summary>
        public decimal EffectiveSaleRate { get; set; }
    }
}
