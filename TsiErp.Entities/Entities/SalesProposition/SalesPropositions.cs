using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;
using TsiErp.Entities.Entities.Branch;
using TsiErp.Entities.Entities.Currency;
using TsiErp.Entities.Entities.CurrentAccountCard;
using TsiErp.Entities.Entities.PaymentPlan;
using TsiErp.Entities.Entities.ProductionOrder;
using TsiErp.Entities.Entities.SalesPropositionLine;
using TsiErp.Entities.Entities.ShippingAdress;
using TsiErp.Entities.Entities.WareHouse;
using Tsi.Core.Utilities.SqlDataTypeMappingUtilities;
using SqlDataType = Tsi.Core.Utilities.SqlDataTypeMappingUtilities.SqlDataType;
using TsiErp.Entities.Entities.WorkOrder;
using TsiErp.Entities.Enums;

namespace TsiErp.Entities.Entities.SalesProposition
{
    /// <summary>
    /// Satış Teklifleri
    /// </summary>
    public class SalesPropositions : FullAuditedEntity
    {
        [SqlColumnType(MaxLength = 17, Nullable = false, SqlDbType = SqlDataType.NVarChar)]
        /// <summary>
        /// Teklif Fiş No
        /// </summary>
        public string FicheNo { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.DateTime)]
        /// <summary>
        /// Tarih
        /// </summary>
        public DateTime Date_ { get; set; }
        [SqlColumnType(MaxLength = 20, Nullable = true, SqlDbType = SqlDataType.NVarChar)]
        /// <summary>
        /// Saat
        /// </summary>
        public string Time_ { get; set; }
        [SqlColumnType(SqlDbType = SqlDataType.Decimal, Precision = 6, Scale = 18)]
        /// <summary>
        /// Kur Tutarı
        /// </summary>
        public decimal ExchangeRate { get; set; }
        [SqlColumnType( Nullable = true, SqlDbType = SqlDataType.NVarCharMax)]
        /// <summary>
        /// Açıklama
        /// </summary>
        public string Description_ { get; set; }
        [SqlColumnType(MaxLength = 201, Nullable = true, SqlDbType = SqlDataType.NVarChar)]
        /// <summary>
        /// Özel Kod
        /// </summary>
        public string SpecialCode { get; set; }
        [SqlColumnType(MaxLength = 50, Nullable = true, SqlDbType = SqlDataType.NVarChar)]
        /// <summary>
        /// Teklif Revizyon No
        /// </summary>
        public string PropositionRevisionNo { get; set; }
        [SqlColumnType(Nullable = true, SqlDbType = SqlDataType.DateTime)]
        /// <summary>
        /// Revizyon Tarihi
        /// </summary>
        public DateTime? RevisionDate { get; set; }
        [SqlColumnType(MaxLength = 8, Nullable = true, SqlDbType = SqlDataType.NVarChar)]
        ///<summary>
        /// Revizyon Saati
        /// </summary>
        public string RevisionTime { get; set; }
        [SqlColumnType( SqlDbType = SqlDataType.Int)]
        /// <summary>
        /// Satış Teklif Durumu
        /// </summary>
        public SalesPropositionStateEnum SalesPropositionState { get; set; }
        [SqlColumnType(Nullable = true, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// Bağlı Teklif ID
        /// </summary>
        public Guid LinkedSalesPropositionID { get; set; }
        [SqlColumnType(SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// Para Birimi ID
        /// </summary>
        public Guid CurrencyID { get; set; }
        [SqlColumnType(SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// Ödeme Planı ID
        /// </summary>
        public Guid PaymentPlanID { get; set; }
        [SqlColumnType(SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// Şube ID
        /// </summary>
        public Guid BranchID { get; set; }
        [SqlColumnType(SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// Depo ID
        /// </summary>
        public Guid WarehouseID { get; set; }
        [SqlColumnType(SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// Cari Hesap Kartı ID
        /// </summary>
        public Guid CurrentAccountCardID { get; set; }
        [SqlColumnType(SqlDbType = SqlDataType.Decimal, Precision = 6, Scale = 18)]
        /// <summary>
        /// Brüt Tutar
        /// </summary>
        public decimal GrossAmount { get; set; }
        [SqlColumnType(SqlDbType = SqlDataType.Decimal, Precision = 6, Scale = 18)]
        /// <summary>
        /// KDV hariç Tutar
        /// </summary>
        public decimal TotalVatExcludedAmount { get; set; }
        [SqlColumnType(SqlDbType = SqlDataType.Decimal, Precision = 6, Scale = 18)]
        /// <summary>
        /// KDV Tutar
        /// </summary>
        public decimal TotalVatAmount { get; set; }
        [SqlColumnType(SqlDbType = SqlDataType.Decimal, Precision = 6, Scale = 18)]
        /// <summary>
        /// Toplam İndirimli Tutar
        /// </summary>
        public decimal TotalDiscountAmount { get; set; }
        [SqlColumnType(SqlDbType = SqlDataType.Decimal, Precision = 6, Scale = 18)]
        /// <summary>
        /// Net Tutar
        /// </summary>
        public decimal NetAmount { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.DateTime)]
        /// <summary>
        /// Geçerlilik Tarihi
        /// </summary>
        public DateTime ValidityDate_ { get; set; }
        [SqlColumnType(Nullable = true, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// Sevkiyat Adresi ID
        /// </summary>
        public Guid? ShippingAdressID { get; set; }
    }
}
