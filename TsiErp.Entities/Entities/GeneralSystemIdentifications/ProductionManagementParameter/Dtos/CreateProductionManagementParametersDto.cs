using Tsi.Core.Entities;
using Tsi.Core.Entities.Auditing;


namespace TsiErp.Entities.Entities.GeneralSystemIdentifications.ProductionManagementParameter.Dtos
{
    public class CreateProductionManagementParametersDto : IEntityDto
    {
        /// <summary>
        /// İleri Zamanlı Tarih Parametresi
        /// </summary>
        public bool FutureDateParameter { get; set; }
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Özkütle
        /// </summary>
        public decimal Density_ { get; set; }
        /// <summary>
        /// Varsayılan Şube
        /// </summary>
        public Guid DefaultBranchID { get; set; }
        /// <summary>
        /// Varsayılan Depo
        /// </summary>
        public Guid DefaultWarehouseID { get; set; }
    }
}
