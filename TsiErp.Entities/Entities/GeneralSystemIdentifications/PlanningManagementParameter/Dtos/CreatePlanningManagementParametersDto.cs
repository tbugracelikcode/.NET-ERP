using Tsi.Core.Entities;
using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.GeneralSystemIdentifications.PlanningManagementParameter.Dtos
{
    public class CreatePlanningManagementParametersDto : IEntityDto
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
        /// MRP Satınalma Sürecinin Başlangıç Parametresi
        /// </summary>
        public int MRPPurchaseTransaction { get; set; }
        /// <summary>
        /// MRPII Kaynak Modül Parametresi
        /// </summary>
        public int MRPIISourceModule { get; set; }
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
