using Tsi.Core.Entities.Auditing;
using Tsi.Core.Entities;

namespace TsiErp.Entities.Entities.GeneralSystemIdentifications.StockManagementParameter.Dtos
{
    public class ListStockManagementParametersDto : IEntityDto
    {
        /// <summary>
        /// İleri Zamanlı Tarih Parametresi
        /// </summary>
        public bool FutureDateParameter { get; set; }
        /// <summary>
        /// Otomatik Maliyet Parametresi
        /// </summary>
        public bool AutoCostParameter { get; set; }
        /// <summary>
        /// Maliyet Hesaplama Yöntemi
        /// </summary>
        public int CostCalculationMethod { get; set; }
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Varsayılan Şube
        /// </summary>
        public Guid DefaultBranchID { get; set; }
        /// <summary>
        /// Varsayılan Şube Kodu
        /// </summary>
        public string DefaultBranchCode { get; set; }
        /// <summary>
        /// Varsayılan Depo
        /// </summary>
        public Guid DefaultWarehouseID { get; set; }
        /// <summary>
        /// Varsayılan Depo Kodu
        /// </summary>
        public string DefaultWarehouseCode { get; set; }
    }
}
