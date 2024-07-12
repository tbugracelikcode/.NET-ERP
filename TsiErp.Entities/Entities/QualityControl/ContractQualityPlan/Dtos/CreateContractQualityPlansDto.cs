using Tsi.Core.Entities.Auditing;
using TSI.QueryBuilder.MappingAttributes;
using TsiErp.Entities.Entities.QualityControl.ContractOperationPicture.Dtos;
using TsiErp.Entities.Entities.QualityControl.ContractQualityPlanLine.Dtos;
using TsiErp.Entities.Entities.QualityControl.ContractQualityPlanOperation.Dtos;

namespace TsiErp.Entities.Entities.QualityControl.ContractQualityPlan.Dtos
{
    public class CreateContractQualityPlansDto : FullAuditedEntityDto
    {
        /// <summary>
        /// Ürün ID
        /// </summary>
        public Guid? ProductID { get; set; }
        ///<summary>
        /// Revizyon No
        /// </summary
        public string RevisionNo { get; set; }

        ///<summary>
        ///Cari Hesap ID
        /// </summary
        public Guid? CurrrentAccountCardID { get; set; }

        /// <summary>
        /// Döküman Numarası
        /// </summary>
        public string DocumentNumber { get; set; }

        /// <summary>
        /// Parti İçi Numune Sayısı
        /// </summary>
        public int NumberofSampleinPart { get; set; }

        /// <summary>
        /// Kabul Edilebilir Hatalı Ürün Sayısı
        /// </summary>
        public int AcceptableNumberofDefectiveProduct { get; set; }

        ///<summary>
        ///Açıklama
        /// </summary
        public string Description_ { get; set; }

        [NoDatabaseAction]
        public List<SelectContractQualityPlanLinesDto> SelectContractQualityPlanLines { get; set; }

        [NoDatabaseAction]
        public List<SelectContractOperationPicturesDto> SelectContractOperationPictures { get; set; }
        [NoDatabaseAction]
        public List<SelectContractQualityPlanOperationsDto> SelectContractQualityPlanOperations { get; set; }
    }
}
