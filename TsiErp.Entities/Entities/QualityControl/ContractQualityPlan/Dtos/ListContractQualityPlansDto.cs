using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.QualityControl.ContractQualityPlan.Dtos
{
    public class ListContractQualityPlansDto : FullAuditedEntityDto
    {
        /// <summary>
        /// Ürün Kodu
        /// </summary>
        public string ProductCode { get; set; }
        /// <summary>
        /// Ürün Açıklaması
        /// </summary>
        public string ProductName { get; set; }
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

       
    }
}
