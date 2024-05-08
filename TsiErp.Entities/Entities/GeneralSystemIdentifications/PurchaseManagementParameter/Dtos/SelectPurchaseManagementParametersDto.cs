using Tsi.Core.Entities;


namespace TsiErp.Entities.Entities.GeneralSystemIdentifications.PurchaseManagementParameter.Dtos
{
    public class SelectPurchaseManagementParametersDto : IEntityDto
    {
        /// <summary>
        /// Sipariş İleri Zamanlı Tarih Parametresi
        /// </summary>
        public bool OrderFutureDateParameter { get; set; }
        /// <summary>
        /// Talep İleri Zamanlı Tarih Parametresi
        /// </summary>
        public bool RequestFutureDateParameter { get; set; }
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Şube ID
        /// </summary>
        public Guid? BranchID { get; set; }
        /// <summary>
        /// Şube Kodu
        /// </summary>
        public string BranchCode { get; set; }
        /// <summary>
        /// Depo ID
        /// </summary>
        public Guid? WarehouseID { get; set; }
        /// <summary>
        /// Depo Kodu
        /// </summary>
        public string WarehouseCode { get; set; }
        /// <summary>
        /// Satın Alma Sipariş Kur Türü
        /// </summary>
        public int PurchaseOrderExchangeRateType { get; set; }
        /// <summary>
        /// Satın Alma Talep Kur Türü
        /// </summary>
        public int PurchaseRequestExchangeRateType { get; set; }
    }
}
