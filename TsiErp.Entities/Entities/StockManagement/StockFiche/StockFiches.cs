using Tsi.Core.Entities.Auditing;
using Tsi.Core.Utilities.SqlDataTypeMappingUtilities;
using TsiErp.Entities.Enums;
using SqlDataType = Tsi.Core.Utilities.SqlDataTypeMappingUtilities.SqlDataType;

namespace TsiErp.Entities.Entities.StockManagement.StockFiche
{
    public class StockFiches : FullAuditedEntity
    {
        [SqlColumnType(MaxLength = 17, Nullable = false, SqlDbType = SqlDataType.NVarChar)]
        /// <summary>
        /// Satış Fiş No
        /// </summary>
        public string FicheNo { get; set; }

        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.DateTime)]
        /// <summary>
        /// Tarih
        /// </summary>
        public DateTime Date_ { get; set; }

        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.Time)]
        /// <summary>
        /// Saat
        /// </summary>
        public TimeSpan? Time_ { get; set; }


        [SqlColumnType(Nullable = true, SqlDbType = SqlDataType.NVarCharMax)]
        /// <summary>
        /// Açıklama
        /// </summary>
        public string Description_ { get; set; }

        [SqlColumnType(MaxLength = 50, Nullable = true, SqlDbType = SqlDataType.NVarChar)]
        /// <summary>
        /// Özel Kod
        /// </summary>
        public string SpecialCode { get; set; }

        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.Int)]
        /// <summary>
        /// Fiş Türü
        /// </summary>
        public StockFicheTypeEnum FicheType { get; set; }

        [SqlColumnType(Nullable = true, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// Şube ID
        /// </summary>
        public Guid BranchID { get; set; }


        [SqlColumnType(Nullable = true, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// Depo ID
        /// </summary>
        public Guid WarehouseID { get; set; }

        [SqlColumnType(Nullable = true, SqlDbType = SqlDataType.Int)]
        /// <summary>
        /// Giriş Çıkış Kodu
        /// </summary>
        public int InputOutputCode { get; set; }


        [SqlColumnType(Nullable = true, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// Üretim Emri ID
        /// </summary>
        public Guid ProductionOrderID { get; set; }


        [SqlColumnType(Nullable = true, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// Para Birimi ID
        /// </summary>
        public Guid CurrencyID { get; set; }


        [SqlColumnType(SqlDbType = SqlDataType.Decimal, Precision = 6, Scale = 18)]
        /// <summary>
        /// Kur Tutarı
        /// </summary>
        public decimal ExchangeRate { get; set; }

        [SqlColumnType(SqlDbType = SqlDataType.Decimal, Precision = 6, Scale = 18)]
        /// <summary>
        /// Net Tutar
        /// </summary>
        public decimal NetAmount { get; set; }

    }
}
