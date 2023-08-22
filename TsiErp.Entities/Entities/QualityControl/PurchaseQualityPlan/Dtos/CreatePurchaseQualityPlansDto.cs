using Tsi.Core.Entities.Auditing;
using TSI.QueryBuilder.MappingAttributes;
using TsiErp.Entities.Entities.QualityControl.PurchaseQualityPlanLine.Dtos;

namespace TsiErp.Entities.Entities.QualityControl.PurchaseQualityPlan.Dtos
{
    public class CreatePurchaseQualityPlansDto : FullAuditedEntityDto
    {
        /// <summary>
        /// Ürün ID
        /// </summary>
        public Guid? ProductID { get; set; }

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
        public List<SelectPurchaseQualityPlanLinesDto> SelectPurchaseQualityPlanLines { get; set; }
    }
}
