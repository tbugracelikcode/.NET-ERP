using Tsi.Core.Entities.Auditing;
using TSI.QueryBuilder.MappingAttributes;
using TsiErp.Entities.Entities.SalesManagement.OrderAcceptanceRecordLine.Dtos;

namespace TsiErp.Entities.Entities.SalesManagement.OrderAcceptanceRecord.Dtos
{
    public class UpdateOrderAcceptanceRecordsDto : FullAuditedEntityDto
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
        /// Müşteri Sipariş No
        /// </summary>
        public string CustomerOrderNo { get; set; }
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
        /// Kur Tutarı
        /// </summary>
        public decimal ExchangeRateAmount { get; set; }
        /// <summary>
        /// Sipariş Onay Durumu
        /// </summary>
        public int OrderAcceptanceRecordState { get; set; }

        [NoDatabaseAction]
        public List<SelectOrderAcceptanceRecordLinesDto> SelectOrderAcceptanceRecordLines { get; set; }
    }
}
