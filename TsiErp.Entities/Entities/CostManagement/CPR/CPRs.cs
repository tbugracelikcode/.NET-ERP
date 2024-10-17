using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;
using Tsi.Core.Utilities.SqlDataTypeMappingUtilities;
using TsiErp.Entities.Enums;

namespace TsiErp.Entities.Entities.CostManagement.CPR
{
    public class CPRs : FullAuditedEntity
    {
        [SqlColumnType(MaxLength = 17, Nullable = false, SqlDbType = SqlDataType.NVarChar)]
        /// <summary>
        /// CPR Kod
        /// </summary>
        public string Code { get; set; }
        [SqlColumnType(MaxLength = 200, Nullable = true, SqlDbType = SqlDataType.NVarChar)]
        /// <summary>
        /// Part No
        /// </summary>
        public string PartNo { get; set; }
        [SqlColumnType( Nullable = false, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// Stok ID
        /// </summary>
        public Guid ProductID { get; set; }
        [SqlColumnType(SqlDbType = SqlDataType.Decimal, Precision = 6, Scale = 18)]
        /// <summary>
        /// Tepe Hacim
        /// </summary>
        public decimal PeakVolume { get; set; }
        [SqlColumnType(SqlDbType = SqlDataType.Int)]
        /// <summary>
        /// Incoterms
        /// </summary>
        public CPRsIncotermEnum Incoterms { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// Alıcı ID
        /// </summary>
        public Guid RecieverID { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// Tedarikçi ID
        /// </summary>
        public Guid SupplierID { get; set; }
        [SqlColumnType(MaxLength = 250, Nullable = true, SqlDbType = SqlDataType.NVarChar)]
        /// <summary>
        /// Üretim Yeri
        /// </summary>
        public string ManufacturingLocation { get; set; }
        [SqlColumnType(SqlDbType = SqlDataType.Int)]
        /// <summary>
        /// Yıllık Üretim Saati
        /// </summary>
        public int ProductionHoursperYear { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// Para Birimi ID
        /// </summary>
        public Guid CurrencyID { get; set; }
        [SqlColumnType(SqlDbType = SqlDataType.Int)]
        /// <summary>
        /// Miktar
        /// </summary>
        public int Quantity { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.DateTime)]
        /// <summary>
        /// Tarih
        /// </summary>
        public DateTime Date_ { get; set; }
        [SqlColumnType(MaxLength = 250, Nullable = true, SqlDbType = SqlDataType.NVarChar)]
        /// <summary>
        /// Tedarikçi Yetkili
        /// </summary>
        public string SupplierContact { get; set; }
        [SqlColumnType(MaxLength = 250, Nullable = true, SqlDbType = SqlDataType.NVarChar)]
        /// <summary>
        /// Schaeffler Yetkili
        /// </summary>
        public string RecieverContact { get; set; }
        [SqlColumnType(MaxLength = 250, Nullable = true, SqlDbType = SqlDataType.NVarChar)]
        /// <summary>
        /// Fiyat Azaltma Adımları
        /// </summary>
        public string PriceReductionSteps { get; set; }


        [SqlColumnType(SqlDbType = SqlDataType.Decimal, Precision = 6, Scale = 18)]
        /// <summary>
        /// Toplam Malzeme Maliyeti
        /// </summary>
        public decimal SubtotalMaterialCost { get; set; }
        [SqlColumnType(SqlDbType = SqlDataType.Decimal, Precision = 6, Scale = 18)]
        /// <summary>
        /// Toplam Malzeme Hurda Maliyeti
        /// </summary>
        public decimal SubtotalMaterialScrapCost { get; set; }
        [SqlColumnType(SqlDbType = SqlDataType.Decimal, Precision = 6, Scale = 18)]
        /// <summary>
        /// Toplam Üretim Maliyeti
        /// </summary>
        public decimal SubtotalManufacturingCost { get; set; }
        [SqlColumnType(SqlDbType = SqlDataType.Decimal, Precision = 6, Scale = 18)]
        /// <summary>
        /// Toplam Üretim Hurda Maliyeti
        /// </summary>
        public decimal SubtotalManufacturingScrapCost { get; set; }
        [SqlColumnType(SqlDbType = SqlDataType.Decimal, Precision = 6, Scale = 18)]
        /// <summary>
        /// Toplam Ayar Maliyeti
        /// </summary>
        public decimal SubtotalSetupCost { get; set; }
    }
}
