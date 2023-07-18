using Tsi.Core.Entities.Auditing;


namespace TsiErp.Entities.Entities.GeneralSystemIdentifications.ShippingManagementParameter.Dtos
{
    public class SelectShippingManagementParametersDto : FullAuditedEntityDto
    {
        /// <summary>
        /// İleri Zamanlı Tarih Parametresi
        /// </summary>
        public bool FutureDateParameter { get; set; }
    }
}
