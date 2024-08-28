using Tsi.Core.Entities;


namespace TsiErp.Entities.Entities.GeneralSystemIdentifications.SalesManagementParameter.Dtos
{
    public class ListSalesManagementParametersDto : IEntityDto
    {
        /// <summary>
        /// Sipariş İleri Zamanlı Tarih Parametresi
        /// </summary>
        public bool OrderFutureDateParameter { get; set; }
        /// <summary>
        /// Teklif İleri Zamanlı Tarih Parametresi
        /// </summary>
        public bool PropositionFutureDateParameter { get; set; }
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Satış Sipariş Kur Türü
        /// </summary>
        public int SalesOrderExchangeRateType { get; set; }
        /// <summary>
        /// Verilen Teklif Kur Türü
        /// </summary>
        public int SalesPropositionExchangeRateType { get; set; }
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
    }
}
