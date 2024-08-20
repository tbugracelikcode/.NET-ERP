using Tsi.Core.Entities.Auditing;
using TSI.QueryBuilder.MappingAttributes;
using TsiErp.Entities.Entities.QualityControl.OperationalQualityPlanLine.Dtos;
using TsiErp.Entities.Entities.QualityControl.OperationPicture.Dtos;

namespace TsiErp.Entities.Entities.QualityControl.OperationalQualityPlan.Dtos
{
    public class SelectOperationalQualityPlansDto : FullAuditedEntityDto
    {
        /// <summary>
        /// Ürün ID
        /// </summary>
        public Guid ProductID { get; set; }
        /// <summary>
        /// Ürün Kodu
        /// </summary>

        public string ProductCode { get; set; }

        /// <summary>
        /// Ürün Adı
        /// </summary>

        public string ProductName { get; set; }
        ///<summary>
        ///Operasyon ID
        /// </summary
        public Guid ProductsOperationID { get; set; }

        ///<summary>
        ///Açıklama
        /// </summary
        public string Description_ { get; set; }

        /// <summary>
        /// Operasyon Kodu
        /// </summary>
        public string OperationCode { get; set; }

        /// <summary>
        /// Operasyon Adı
        /// </summary>
        public string OperationName { get; set; }
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
