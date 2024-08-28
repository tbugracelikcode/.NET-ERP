using Tsi.Core.Entities.Auditing;
using TSI.QueryBuilder.MappingAttributes;
using TsiErp.Entities.Entities.SalesManagement.OrderAcceptanceRecordLine.Dtos;
using TsiErp.Entities.Enums;

namespace TsiErp.Entities.Entities.SalesManagement.OrderAcceptanceRecord.Dtos
{
    public class SelectOrderAcceptanceRecordsDto : FullAuditedEntityDto
    {
        /// <summary>
        /// Sipariş Kabul Kod
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Tarih
        /// </summary>
        public DateTime Date_ { get; set; }
        /// <summary>
        /// Cari Hesap ID
        /// </summary>
        public Guid? CurrentAccountCardID { get; set; }
        /// <summary>
        /// Cari Hesap Kodu
        /// </summary>
        public string CurrentAccountCardCode { get; set; }
        /// <summary>
        /// Cari Hesap Ünvanı
        /// </summary>
        public string CurrentAccountCardName { get; set; }
        /// <summary>
        /// Cari Hesap Müşteri Kodu
        /// </summary>
        public string CurrentAccountCardCustomerCode { get; set; }
        /// <summary>
        /// Müşteri Sipariş No
        /// </summary>
        public string CustomerOrderNo { get; set; }
        /// <summary>
        /// Açıklama
        /// </summary>
        public string Description_ { get; set; }
        /// <summary>
        /// Müşterinin İstediği Tarih
        /// </summary>
        public DateTime CustomerRequestedDate { get; set; }
        /// <summary>
        /// Teyit Edilen Yükleme Tarihi
        /// </summary>
        public DateTime ConfirmedLoadingDate { get; set; }
        /// <summary>
        /// Üretim Emri Yükleme Tarihi
        /// </summary>
        public DateTime ProductionOrderLoadingDate { get; set; }
        /// <summary>
        /// Para Birimi ID
        /// </summary>
        public Guid? CurrenyID { get; set; }
        /// <summary>
        /// Para Birimi Kodu
        /// </summary>
        public string CurrenyCode { get; set; }
        /// <summary>
        /// Kur Tutarı
        /// </summary>
        public decimal ExchangeRateAmount { get; set; }
        /// <summary>
        /// Sipariş Onay Durumu
        /// </summary>
        public OrderAcceptanceRecordStateEnum OrderAcceptanceRecordState { get; set; }
        /// <summary>
        /// Ödeme Planı ID
        /// </summary>
        public Guid PaymentPlanID { get; set; }
        /// <summary>
        /// Ödeme Planı Adı
        /// </summary>
        public string PaymentPlanName { get; set; }

        [NoDatabaseAction]
        public List<SelectOrderAcceptanceRecordLinesDto> SelectOrderAcceptanceRecordLines { get; set; }
    }
}
