using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.QualityControl.ContractQualityPlanLine.Dtos
{
    public class ListContractQualityPlanLinesDto : FullAuditedEntityDto
    {
        /// <summary>
        /// Ürün Kodu
        /// </summary>
        public string ProductCode { get; set; }

        /// <summary>
        /// Ürün Adı
        /// </summary>
        public string ProductName { get; set; }
        /// <summary>
        /// Satır No
        /// </summary>
        public int LineNr { get; set; }

        /// <summary>
        /// Kalite Planı Kodu
        /// </summary>
        public string Code { get; set; }

        ///<summary>
        ///Kontrol Türü
        /// </summary
        public string ControlTypesName { get; set; }

        ///<summary>
        ///İş Merkezi
        /// </summary
        public string WorkCenterName { get; set; }
    }
}
