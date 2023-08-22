using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.QualityControl.OperationPicture.Dtos
{
    public class ListOperationPicturesParameterDto : FullAuditedEntity
    {
        /// <summary>
        /// Operasyon Kalite Planı ID
        /// </summary>
        public Guid OperationalQualityPlanID { get; set; }
    }
}
