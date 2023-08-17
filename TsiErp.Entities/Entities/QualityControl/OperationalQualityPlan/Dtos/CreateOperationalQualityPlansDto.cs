using Tsi.Core.Entities.Auditing;
using TSI.QueryBuilder.MappingAttributes;
using TsiErp.Entities.Entities.QualityControl.OperationalQualityPlanLine.Dtos;
using TsiErp.Entities.Entities.QualityControl.OperationPicture.Dtos;

namespace TsiErp.Entities.Entities.QualityControl.OperationalQualityPlan.Dtos
{
    public class CreateOperationalQualityPlansDto : FullAuditedEntityDto
    {
        /// <summary>
        /// Ürün ID
        /// </summary>
        public Guid? ProductID { get; set; }

        ///<summary>
        ///Operasyon ID
        /// </summary
        public Guid? ProductsOperationID { get; set; }

        ///<summary>
        ///Açıklama
        /// </summary
        public string Description_ { get; set; }
        /// <summary>
        /// Döküman Numarası
        /// </summary>
        public string DocumentNumber { get; set; }


        [NoDatabaseAction]
        public List<SelectOperationalQualityPlanLinesDto> SelectOperationalQualityPlanLines { get; set; }

        [NoDatabaseAction]
        public List<SelectOperationPicturesDto> SelectOperationPictures { get; set; }
    }
}
