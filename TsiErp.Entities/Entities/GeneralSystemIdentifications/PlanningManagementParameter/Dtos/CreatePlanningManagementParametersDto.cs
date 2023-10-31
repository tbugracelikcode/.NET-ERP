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
    }
}
