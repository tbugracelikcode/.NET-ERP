using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;
using Tsi.Core.Utilities.SqlDataTypeMappingUtilities;

namespace TsiErp.Entities.Entities.QualityControl.UnsuitabilityItemSPCLine.Dtos
{
    public class ListUnsuitabilityItemSPCLinesDto : FullAuditedEntityDto
    {
        /// <summary>
        /// İş Merkezi Adı
        /// </summary>
        public string WorkCenterName { get; set; }
        /// <summary>
        /// Uygunsuzluk Türü Adı
        /// </summary>
        public string UnsuitabilityTypeName { get; set; }
        /// <summary>
        /// Uygunsuzluk Başlığı Adı
        /// </summary>
        public string UnsuitabilityItemName { get; set; }
        /// <summary>
        /// Satır No
        /// </summary>
        public int LineNr { get; set; }
        /// <summary>
        /// Uygunsuzluk Başlığı Şiddet Katsayısı
        /// </summary>
        public int UnsuitabilityItemIntensityCoefficient { get; set; }
        /// <summary>
        /// Toplam Uygunsuz Komponent Sayısı
        /// </summary>
        public int TotalUnsuitableComponent { get; set; }
        /// <summary>
        /// Toplam Uygunsuz Rapor Sayısı
        /// </summary>
        public int TotalUnsuitableReport { get; set; }
        /// <summary>
        /// Rapor Başına Uygunsuz Komponent 
        /// </summary>
        public int UnsuitableComponentPerReport { get; set; }
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
    }
}
