using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;
using TsiErp.Entities.Enums;

namespace TsiErp.Entities.Entities.StockManagement.TechnicalDrawing.Dtos
{
    public class SelectTechnicalDrawingsDto : FullAuditedEntityDto
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
        /// Cari Hesap ID
        /// </summary>
        public Guid? CustomerCurrentAccountCardID { get; set; }
        /// <summary>
        /// Müşteri Kodu
        /// </summary>
        public string CustomerCode { get; set; }
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
        public Guid? ProductID { get; set; }
        /// <summary>
        /// Stok Kodu
        /// </summary>
        public string ProductCode { get; set; }
        /// <summary>
        /// Stok Açıklaması
        /// </summary>
        public string ProductName { get; set; }
        /// <summary>
        /// Stok Açıklaması
        /// </summary>
        public ProductTypeEnum ProductType { get; set; }
        /// <summary>
        /// Dosya adı
        /// </summary>
        public string UploadedFileName { get; set; }
    }
}
