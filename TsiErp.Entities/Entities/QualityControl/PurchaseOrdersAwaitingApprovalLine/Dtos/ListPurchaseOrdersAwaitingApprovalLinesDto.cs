using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.QualityControl.PurchaseOrdersAwaitingApprovalLine.Dtos
{
    public class ListPurchaseOrdersAwaitingApprovalLinesDto : FullAuditedEntityDto
    {
        /// <summary>
        /// Onay Bekleyen Satın Alma Sipariş ID
        /// </summary>
        public Guid PurchaseOrdersAwaitingApprovalID { get; set; }
        /// <summary>
        /// Satır No
        /// </summary>
        public int LineNr { get; set; }
        ///<summary>
        ///Kontrol Türü ID
        /// </summary
        public Guid? ControlTypesID { get; set; }
        ///<summary>
        ///Kontrol Türü Adı
        /// </summary
        public string ControlTypesName { get; set; }
        ///<summary>
        ///Kontrol Sıklığı
        /// </summary
        public string ControlFrequency { get; set; }
        /// <summary>
        /// Olması Gereken Ölçü
        /// </summary>
        public decimal IdealMeasure { get; set; }
        /// <summary>
        /// Alt Tolerans
        /// </summary>
        public decimal BottomTolerance { get; set; }
        /// <summary>
        /// Üst Tolerans
        /// </summary>
        public decimal UpperTolerance { get; set; }
        /// <summary>
        /// Ölçü Değeri
        /// </summary>
        public decimal MeasureValue { get; set; }
    }
}
