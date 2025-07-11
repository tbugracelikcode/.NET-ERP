﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;
using Tsi.Core.Utilities.SqlDataTypeMappingUtilities;

namespace TsiErp.Entities.Entities.QualityControl.UnsuitabilityItemSPCLine.Dtos
{
    public class UpdateUnsuitabilityItemSPCLinesDto : FullAuditedEntityDto
    {
        /// <summary>
        /// SPC ID
        /// </summary>
        public Guid UnsuitabilitySPCID { get; set; }
        /// <summary>
        /// İş Merkezi ID
        /// </summary>
        public Guid? WorkCenterID { get; set; }
        /// <summary>
        /// Uygunsuzluk Türü ID
        /// </summary>
        public Guid? UnsuitabilityTypeID { get; set; }
        /// <summary>
        /// Satır No
        /// </summary>
        public int LineNr { get; set; }
        /// <summary>
        /// Uygunsuzluk Başlığı ID
        /// </summary>
        public Guid? UnsuitabilityItemID { get; set; }
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
