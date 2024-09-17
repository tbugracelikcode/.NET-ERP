using Tsi.Core.Entities;
using TsiErp.Entities.Enums;

namespace TsiErp.Entities.Entities.ProductionManagement.OperationQuantityInformation.Dtos
{
    public class SelectOperationQuantityInformationsDto : IEntityDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// İş Emri ID
        /// </summary>
        public Guid? WorkOrderID { get; set; }
        /// <summary>
        /// İş Emri No
        /// </summary>
        public string WorkOrderNo { get; set; }
        /// <summary>
        /// Üretim Emri ID
        /// </summary>
        public Guid? ProductionOrderID { get; set; }
        /// <summary>
        /// Üretim Emri No
        /// </summary>
        public string ProductionOrderNo { get; set; }
        /// <summary>
        /// Operatör ID
        /// </summary>
        public Guid? OperatorID { get; set; }
        /// <summary>
        /// Operatör Adı
        /// </summary>
        public string OperatorName { get; set; }
        /// <summary>
        /// Bağlama Süresi
        /// </summary>
        public decimal AttachmentTime { get; set; }
        /// <summary>
        /// Operasyon Süresi
        /// </summary>
        public decimal OperationTime { get; set; }
        /// <summary>
        /// Üretim Takip ID
        /// </summary>
        public Guid? ProductionTrackingID { get; set; }
        /// <summary>
        /// Üretim Takip No
        /// </summary>
        public string ProductionTrackingNo { get; set; }
        /// <summary>
        /// İstasyon ID
        /// </summary>
        public Guid? StationID { get; set; }
        /// <summary>
        /// İstasyon Kodu
        /// </summary>
        public string StationCode { get; set; }
        /// <summary>
        /// Operasyon ID
        /// </summary>
        public Guid? ProductsOperationID { get; set; }
        /// <summary>
        /// Operasyon Adı
        /// </summary>
        public string ProductsOperationName { get; set; }
        /// <summary>
        /// Tür
        /// </summary>
        public OperationQuantityInformationsTypeEnum OperationQuantityInformationsType { get; set; }
    }
}
