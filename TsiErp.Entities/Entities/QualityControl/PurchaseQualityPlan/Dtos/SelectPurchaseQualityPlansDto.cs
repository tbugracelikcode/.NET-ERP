using Tsi.Core.Entities.Auditing;
using TSI.QueryBuilder.MappingAttributes;
using TsiErp.Entities.Entities.QualityControl.PurchaseQualityPlanLine.Dtos;

namespace TsiErp.Entities.Entities.QualityControl.PurchaseQualityPlan.Dtos
{
    public class SelectPurchaseQualityPlansDto : FullAuditedEntityDto
    {
        /// <summary>
        /// Ürün ID
        /// </summary>
        public Guid? ProductID { get; set; }
        /// <summary>
        /// Ürün Kodu
        /// </summary>
        public string ProductCode { get; set; }
        /// <summary>
        /// Ürün Açıklaması
        /// </summary>
        public string ProductName { get; set; }

        ///<summary>
        ///Cari Hesap ID
        /// </summary
        public Guid? CurrrentAccountCardID { get; set; }
        ///<summary>
        ///Cari Hesap Kodu
        /// </summary
        public string CurrrentAccountCardCode { get; set; }
        ///<summary>
        ///Cari Hesap Ünvanı
        /// </summary
        public string CurrrentAccountCardName { get; set; }

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

        /// <summary>
        /// Yüklenen Domain
        /// </summary>
        public string DrawingDomain { get; set; }

        /// <summary>
        /// Dosya Yolu
        /// </summary>
        public string DrawingFilePath { get; set; }
        /// <summary>
        /// Dosya adı
        /// </summary>
        public string UploadedFileName { get; set; }
        ///<summary>
        /// Revizyon No
        /// </summary
        public string RevisionNo { get; set; }

        [NoDatabaseAction]
        public List<SelectPurchaseQualityPlanLinesDto> SelectPurchaseQualityPlanLines { get; set; }

    }
}
