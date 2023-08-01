using Tsi.Core.Entities;
using Tsi.Core.Entities.Auditing;


namespace TsiErp.Entities.Entities.GeneralSystemIdentifications.StockManagementParameter.Dtos
{
    public class SelectStockManagementParametersDto : IEntityDto
    {
        /// <summary>
        /// İleri Zamanlı Tarih Parametresi
        /// </summary>
        public bool FutureDateParameter { get; set; }
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }
    }
}
