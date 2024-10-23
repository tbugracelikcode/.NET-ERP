using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;
using Tsi.Core.Utilities.SqlDataTypeMappingUtilities;
using TsiErp.Entities.Enums;

namespace TsiErp.Entities.Entities.SalesManagement.SalesInvoice
{
    public class SalesInvoices : FullAuditedEntity
    {
        [SqlColumnType(MaxLength = 17, Nullable = false, SqlDbType = SqlDataType.NVarChar)]
        /// <summary>
        /// Satış Fatura Fiş No
        /// </summary>
        public string FicheNo { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.DateTime)]
        /// <summary>
        /// Tarih
        /// </summary>
        public DateTime Date_ { get; set; }
        [SqlColumnType(Nullable = true, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// İşlem Dövizi ID
        /// </summary>
        public Guid TransactionExchangeCurrencyID { get; set; }
        [SqlColumnType(Nullable = true, SqlDbType = SqlDataType.DateTime)]
        /// <summary>
        /// Müşterinin İstediği Tarih
        /// </summary>
        public DateTime CustomerRequestedDate { get; set; }
        [SqlColumnType(MaxLength = 20, Nullable = true, SqlDbType = SqlDataType.NVarChar)]
        /// <summary>
        /// Saat
        /// </summary>
        public string Time_ { get; set; }
        [SqlColumnType(MaxLength = 250, Nullable = true, SqlDbType = SqlDataType.NVarChar)]
        /// <summary>
        /// Müşteri Sipariş No
        /// </summary>
        public string CustomerOrderNr { get; set; }
        [SqlColumnType(SqlDbType = SqlDataType.Decimal, Precision = 6, Scale = 18)]
        /// <summary>
        /// Kur Tutarı
        /// </summary>
        public decimal ExchangeRate { get; set; }
        [SqlColumnType(Nullable = true, SqlDbType = SqlDataType.NVarCharMax)]
        /// <summary>
        /// Açıklama
        /// </summary>
        public string Description_ { get; set; }
        [SqlColumnType(MaxLength = 201, Nullable = true, SqlDbType = SqlDataType.NVarChar)]
        /// <summary>
        /// Özel Kod
        /// </summary>
        public string SpecialCode { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.Int)]
        /// <summary>
        /// Fatura Durumu
        /// </summary>
        public SalesInvoicesStateEnum SalesInvoicesState { get; set; }
        [SqlColumnType(Nullable = true, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// Bağlı Teklif ID
        /// </summary>
        public Guid LinkedSalesPropositionID { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// Para Birimi ID
        /// </summary>
        public Guid CurrencyID { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// Ödeme Planı ID
        /// </summary>
        public Guid PaymentPlanID { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// Şube ID
        /// </summary>
        public Guid BranchID { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// Depo ID
        /// </summary>
        public Guid WarehouseID { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.UniqueIdentifier)]
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
        [SqlColumnType(Nullable = true, SqlDbType = SqlDataType.DateTime)]
        /// <summary>
        /// Üretim Emri Oluşturulma Tarihi
        /// </summary>
        public DateTime? WorkOrderCreationDate { get; set; }
        [SqlColumnType(Nullable = true, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// Sevkiyat Adresi ID
        /// </summary>
        public Guid? ShippingAdressID { get; set; }
        [SqlColumnType(SqlDbType = SqlDataType.Decimal, Precision = 6, Scale = 18)]
        /// <summary>
        /// İşlem Dövizi Brüt Tutar
        /// </summary>
        public decimal TransactionExchangeGrossAmount { get; set; }

        [SqlColumnType(SqlDbType = SqlDataType.Decimal, Precision = 6, Scale = 18)]
        /// <summary>
        /// İşlem Dövizi Toplam İndirimli Tutar
        /// </summary>
        public decimal TransactionExchangeTotalDiscountAmount { get; set; }
        [SqlColumnType(SqlDbType = SqlDataType.Decimal, Precision = 6, Scale = 18)]
        /// <summary>
        /// İşlem Dövizi Net Tutar
        /// </summary>
        public decimal TransactionExchangeNetAmount { get; set; }
        [SqlColumnType(SqlDbType = SqlDataType.Decimal, Precision = 6, Scale = 18)]
        /// <summary>
        /// İşlem Dövizi KDV Tutar
        /// </summary>
        public decimal TransactionExchangeTotalVatAmount { get; set; }

        [SqlColumnType(SqlDbType = SqlDataType.Decimal, Precision = 6, Scale = 18)]
        /// <summary>
        /// İşlem Dövizi KDV hariç Tutar
        /// </summary>
        public decimal TransactionExchangeTotalVatExcludedAmount { get; set; }

        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.Int)]
        /// <summary>
        ///Fiyatlandırma Dövizi
        /// </summary>
        public PricingCurrencyEnum PricingCurrency { get; set; }
        [SqlColumnType(Nullable = true, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// Sipariş Kabul ID
        /// </summary>
        public Guid? OrderAcceptanceRecordID { get; set; }
        [SqlColumnType(Nullable = true, SqlDbType = SqlDataType.DateTime)]
        /// <summary>
        /// Teyit Edilen Yükleme Tarihi
        /// </summary>
        public DateTime ConfirmedLoadingDate { get; set; }
        [SqlColumnType(SqlDbType = SqlDataType.Bit, Nullable = false)]
        /// <summary>
        /// Standart
        /// </summary>
        public bool isStandart { get; set; }
        [SqlColumnType(Nullable = true, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// Satış Sipariş ID
        /// </summary>
        public Guid SalesOrderID { get; set; }
    }
}
