using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.ProductionManagement.TemplateOperationUnsuitabilityItem.Dtos
{
    public class UpdateTemplateOperationUnsuitabilityItemsDto : FullAuditedEntityDto
    {
        ///<summary>
        ///Şablon Operasyon ID
        /// </summary
        public Guid TemplateOperationId { get; set; }

        ///<summary>
        ///Uygunsuzluk Başlığı ID
        /// </summary
        public Guid UnsuitabilityItemsId { get; set; }

        /// <summary>
        /// Kullanılacak
        /// </summary>
        public bool ToBeUsed { get; set; }

        /// <summary>
        /// Satır Numarası
        /// </summary>
        public int LineNr { get; set; }
    }
}
