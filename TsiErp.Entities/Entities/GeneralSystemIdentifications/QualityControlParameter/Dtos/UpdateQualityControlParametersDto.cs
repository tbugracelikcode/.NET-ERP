using Tsi.Core.Entities.Auditing;


namespace TsiErp.Entities.Entities.GeneralSystemIdentifications.QualityControlParameter.Dtos
{
    public class UpdateQualityControlParametersDto 
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
