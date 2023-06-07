using Tsi.Core.Entities.Auditing;
using Tsi.Core.Utilities.SqlDataTypeMappingUtilities;
using TsiErp.Entities.Enums;
using SqlDataType = Tsi.Core.Utilities.SqlDataTypeMappingUtilities.SqlDataType;

namespace TsiErp.Entities.Entities.Product
{
    /// <summary>
    /// Stok Kodları
    /// </summary>
    public class Products : FullAuditedEntity
    {
        [SqlColumnType(MaxLength = 17, Nullable = false, SqlDbType = SqlDataType.NVarChar)]
        /// <summary>
        /// Kod
        /// </summary>
        public string Code { get; set; }
        [SqlColumnType(MaxLength = 200, Nullable = false, SqlDbType = SqlDataType.NVarChar)]
        /// <summary>
        /// Açıklama
        /// </summary>
        public string Name { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.Bit)]
        /// <summary>
        /// Aktiflik
        /// </summary>
        public bool IsActive { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.Int)]
        /// <summary>
        /// Temin Şekli
        /// </summary>
        public ProductSupplyFormEnum SupplyForm { get; set; }
        [SqlColumnType(SqlDbType = SqlDataType.Decimal, Precision = 6, Scale = 18)]
        /// <summary>
        /// Stok Boyu
        /// </summary>
        public decimal ProductSize { get; set; }
        [SqlColumnType(MaxLength = 50, Nullable = true, SqlDbType = SqlDataType.NVarChar)]
        /// <summary>
        /// GTIP
        /// </summary>
        public string GTIP { get; set; }
        [SqlColumnType(SqlDbType = SqlDataType.Decimal, Precision = 6, Scale = 18)]
        /// <summary>
        /// Testere Fire
        /// </summary>
        public decimal SawWastage { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.Bit)]
        /// <summary>
        /// Onay
        /// </summary>
        public bool Confirmation { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.Bit)]
        /// <summary>
        /// Teknik Onay
        /// </summary>
        public bool TechnicalConfirmation { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.Int)]
        /// <summary>
        /// Stok Türü
        /// </summary>
        public ProductTypeEnum ProductType { get; set; }
        [SqlColumnType(MaxLength = 500, Nullable = true, SqlDbType = SqlDataType.NVarChar)]
        /// <summary>
        /// Stok Açıklaması
        /// </summary>
        public string ProductDescription { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// Ürün Grup ID
        /// </summary>
        public Guid ProductGrpID { get; set; }
        [SqlColumnType(MaxLength = 50, Nullable = true, SqlDbType = SqlDataType.NVarChar)]
        /// <summary>
        /// Üretici Firma Kodu
        /// </summary>
        public string ManufacturerCode { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.Int)]
        /// <summary>
        /// Satış KDV
        /// </summary>
        public int SaleVAT { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.Int)]
        /// <summary>
        /// Satın Alma KDV
        /// </summary>
        public int PurchaseVAT { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// Birim Seti ID
        /// </summary>
        public Guid UnitSetID { get; set; }
        [SqlColumnType(Nullable = true, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// Özellik Set ID
        /// </summary>
        public Guid FeatureSetID { get; set; }
        [SqlColumnType(MaxLength = 201, Nullable = true, SqlDbType = SqlDataType.NVarChar)]
        /// <summary>
        /// İngilizce Tanım
        /// </summary>
        public string EnglishDefinition { get; set; }
        [SqlColumnType(MaxLength = 50, Nullable = true, SqlDbType = SqlDataType.NVarChar)]
        /// <summary>
        /// İhracat Kategori No
        /// </summary>
        public string ExportCatNo { get; set; }
        [SqlColumnType(MaxLength = 50, Nullable = true, SqlDbType = SqlDataType.NVarChar)]
        /// <summary>
        /// OemRefNo
        /// </summary>
        public string OemRefNo { get; set; }
        [SqlColumnType(MaxLength = 50, Nullable = true, SqlDbType = SqlDataType.NVarChar)]
        /// <summary>
        /// OemRefNo2
        /// </summary>
        public string OemRefNo2 { get; set; }
        [SqlColumnType(MaxLength = 50, Nullable = true, SqlDbType = SqlDataType.NVarChar)]
        /// <summary>
        /// OemRefNo3
        /// </summary>
        public string OemRefNo3 { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.Int)]
        /// <summary>
        /// Planlanan Fire
        /// </summary>
        public int PlannedWastage { get; set; }
        [SqlColumnType(SqlDbType = SqlDataType.Decimal, Precision = 6, Scale = 18)]
        /// <summary>
        /// Kaplama Ağırlığı
        /// </summary>
        public decimal CoatingWeight { get; set; }

    }
}
