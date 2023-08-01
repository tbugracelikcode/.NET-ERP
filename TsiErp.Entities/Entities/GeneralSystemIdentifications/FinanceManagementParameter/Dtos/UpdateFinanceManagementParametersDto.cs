using Tsi.Core.Entities.Auditing;
using Tsi.Core.Entities;

namespace TsiErp.Entities.Entities.GeneralSystemIdentifications.FinanceManagementParameter.Dtos
{
    public class UpdateFinanceManagementParametersDto : IEntityDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// İleri Zamanlı Tarih Parametresi
        /// </summary>
        public bool FutureDateParameter { get; set; }
    }
}
