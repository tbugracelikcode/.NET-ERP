using Tsi.Core.Entities;


namespace TsiErp.Entities.Entities.GeneralSystemIdentifications.PurchaseManagementParameter.Dtos
{
    public class ListPurchaseManagementParametersDto : IEntityDto
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
        /// <summary>
        /// Varsayılan Şube
        /// </summary>
        public Guid DefaultBranchID { get; set; }
        /// <summary>
        /// Varsayılan Şube Kodu
        /// </summary>
        public string DefaultBranchCode { get; set; }
        /// <summary>
        /// Varsayılan Şube Kodu
        /// </summary>
        public string DefaultBranchName { get; set; }
        /// <summary>
        /// Varsayılan Depo
        /// </summary>
        public Guid DefaultWarehouseID { get; set; }
        /// <summary>
        /// Varsayılan Depo Kodu
        /// </summary>
        public string DefaultWarehouseCode { get; set; }
        /// <summary>
        /// Varsayılan Depo Kodu
        /// </summary>
        public string DefaultWarehouseName { get; set; }
        /// <summary>
        /// Satın Alma KDV
        /// </summary>
        public int PurchaseVAT { get; set; }
    }
}
