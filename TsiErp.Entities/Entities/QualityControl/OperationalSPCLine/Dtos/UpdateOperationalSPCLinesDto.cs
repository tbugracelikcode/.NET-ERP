using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;
using Tsi.Core.Utilities.SqlDataTypeMappingUtilities;

namespace TsiErp.Entities.Entities.QualityControl.OperationalSPCLine.Dtos
{
    public class UpdateOperationalSPCLinesDto : FullAuditedEntityDto
    {
        /// <summary>
        /// SPC ID
        /// </summary>
        public Guid OperationalSPCID { get; set; }
        /// <summary>
        /// İş Merkezi ID
        /// </summary>
        public Guid? WorkCenterID { get; set; }
        /// <summary>
        /// Operasyon ID
        /// </summary>
        public Guid? OperationID { get; set; }
        /// <summary>
        /// Satır No
        /// </summary>
        public int LineNr { get; set; }
        /// <summary>
        /// Toplam Üretilen Komponent
        /// </summary>
        public int TotalComponent { get; set; }
        /// <summary>
        /// Toplam Uygunsuz Komponent
        /// </summary>
        public int TotalUnsuitableComponent { get; set; }
        /// <summary>
        /// Uygunsuz Komponent Oranı
        /// </summary>
        public int UnsuitableComponentRate { get; set; }

        /// <summary>
        /// Toplam Gerçekleşen Operasyon
        /// </summary>
        public int TotalOccuredOperation { get; set; }
        /// <summary>
        /// Toplam Uygunsuz Operasyon
        /// </summary>
        public int TotalUnsuitableOperation { get; set; }
        /// <summary>
        /// Uygunsuz Operasyon Oranı
        /// </summary>
        public int UnsuitableOperationRate { get; set; }
        /// <summary>
        /// Operasyon Başına Uygunsuz Komponent Sayısı
        /// </summary>
        public int UnsuitableComponentPerOperation { get; set; }
        /// <summary>
        /// Şiddet
        /// </summary>
        public int Severity { get; set; }
        /// <summary>
        /// Sıklık
        /// </summary>
        public int Frequency { get; set; }
        /// <summary>
        /// Farkedilebilirlik
        /// </summary>
        public int Detectability { get; set; }
        /// <summary>
        /// RPN
        /// </summary>
        public int RPN { get; set; }
        /// <summary>
        /// Opersayon Bazlı Ara Kontrol Sıklıkları
        /// </summary>
        public int OperationBasedMidControlFrequency { get; set; }
    }
}
