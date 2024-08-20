using BlazorInputFile;
using Tsi.Core.Entities.Auditing;
using TSI.QueryBuilder.MappingAttributes;

namespace TsiErp.Entities.Entities.QualityControl.OperationPicture.Dtos
{
    public class SelectOperationPicturesDto : FullAuditedEntityDto
    {
        /// <summary>
        /// Operasyon Kalite Planı ID
        /// </summary>
        public Guid OperationalQualityPlanID { get; set; }

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

    }
}
