using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;
using Tsi.Core.Utilities.SqlDataTypeMappingUtilities;

namespace TsiErp.Entities.Entities.ShippingManagement.PackageFiche
{
    public class PackageFiches : FullAuditedEntity
    {
        [SqlColumnType(MaxLength = 17, Nullable = false, SqlDbType = SqlDataType.NVarChar)]
        /// <summary>
        /// Paket Fişi Kodu
        /// </summary>
        public string Code { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.DateTime)]
        /// <summary>
        /// Paket Fişi Tarihi
        /// </summary>
        public DateTime? Date_ { get; set; }
        [SqlColumnType( Nullable = false, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// Satış Sipariş ID
        /// </summary>
        public Guid SalesOrderID { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// Stok ID
        /// </summary>
        public Guid ProductID { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// Cari Hesap ID
        /// </summary>
        public Guid CurrentAccountID { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// Çeki Listesi ID
        /// </summary>
        public Guid PackingListID { get; set; }
        [SqlColumnType(MaxLength = 250, Nullable = true, SqlDbType = SqlDataType.NVarChar)]
        /// <summary>
        /// Koli Türü
        /// </summary>
        public string PackageType { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.Int)]
        /// <summary>
        /// Koli İçeriği
        /// </summary>
        public int PackageContent { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.Int)]
        /// <summary>
        /// Koli Sayısı
        /// </summary>
        public int NumberofPackage { get; set; }
        [SqlColumnType(MaxLength = 250, Nullable = true, SqlDbType = SqlDataType.NVarChar)]
        /// <summary>
        /// Palet No
        /// </summary>
        public string PalletNumber { get; set; }
        [SqlColumnType(MaxLength = 250, Nullable = true, SqlDbType = SqlDataType.NVarChar)]
        /// <summary>
        /// Ürün Palet Sırası
        /// </summary>
        public string ProductPalletOrder { get; set; }
        [SqlColumnType(MaxLength = 250, Nullable = true, SqlDbType = SqlDataType.NVarChar)]
        /// <summary>
        /// Üretim Emri Ref No
        /// </summary>
        public string ProductionOrderReferenceNo { get; set; }
        [SqlColumnType(SqlDbType = SqlDataType.Decimal, Precision = 6, Scale = 18)]
        /// <summary>
        /// Birim Ağırlık
        /// </summary>
        public decimal UnitWeight { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// Üretim Emri ID
        /// </summary>
        public Guid ProductionOrderID { get; set; }
    }
}
