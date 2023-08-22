using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.QualityControl.ContractOperationPicture.Dtos
{
    public class SelectContractOperationPicturesDto : FullAuditedEntityDto
    {
        /// <summary>
        /// Fason Kalite Planı ID
        /// </summary>
        public Guid ContractQualityPlanID { get; set; }

        /// <summary>
        /// Eklenme Tarihi
        /// </summary>
        public DateTime CreationDate_ { get; set; }

        ///<summary>
        /// Çizen
        /// </summary
        public string Drawer { get; set; }
        /// <summary>
        /// Satır No
        /// </summary>
        public int LineNr { get; set; }

        ///<summary>
        /// Onaylayan
        /// </summary
        public string Approver { get; set; }

        /// <summary>
        /// Onaylı
        /// </summary>
        public bool IsApproved { get; set; }
        ///<summary>
        ///Açıklama
        /// </summary
        public string Description_ { get; set; }
    }
}
