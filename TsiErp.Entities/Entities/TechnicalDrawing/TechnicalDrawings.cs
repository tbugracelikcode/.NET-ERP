using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;
using TsiErp.Entities.Entities.Product;

namespace TsiErp.Entities.Entities.TechnicalDrawing
{
    /// <summary>
    /// Teknik Resimler
    /// </summary>
    public class TechnicalDrawings : FullAuditedEntity
    {
        /// <summary>
        /// Revizyon No
        /// </summary>
        public string RevisionNo { get; set; }
        /// <summary>
        /// Revizyon Tarihi
        /// </summary>
        public DateTime? RevisionDate { get; set; }
        /// <summary>
        /// Çizen
        /// </summary>
        public string Drawer { get; set; }
        /// <summary>
        /// Çizim No
        /// </summary>
        public string DrawingNo { get; set; }
        /// <summary>
        /// Açıklama
        /// </summary>
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
        /// Müşteri Onay
        /// </summary>
        public bool CustomerApproval { get; set; }
        /// <summary>
        /// Firma Onay
        /// </summary>
        public bool IsApproved { get; set; }
        /// <summary>
        /// Numune Onay
        /// </summary>
        public bool SampleApproval { get; set; }
        /// <summary>
        /// Stok ID
        /// </summary>
        public Guid ProductID { get; set; }

        public Products Products { get; set; }
    }
}
