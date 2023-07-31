using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.GeneralSystemIdentifications.GeneralParameter.Dtos
{
    public class ListGeneralParametersDto : FullAuditedEntityDto
    {
        /// <summary>
        /// Sayfa Adı
        /// </summary>
        public string PageName { get; set; }
    }
}
