using Tsi.Core.Entities.Auditing;


namespace TsiErp.Entities.Entities.GeneralSystemIdentifications.ProductionManagementParameter.Dtos
{
    public class SelectProductionManagementParametersDto 
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
