using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.QualityControl.PurchaseUnsuitabilityReport.Dtos
{
    public class CreatePurchaseUnsuitabilityReportsDto : FullAuditedEntityDto
    {
        /// <summary>
        /// Rapor Fiş No
        /// </summary>
        public string FicheNo { get; set; }
        /// <summary>
        /// Parti No
        /// </summary>
        public string PartyNo { get; set; }
        /// <summary>
        /// Tarih
        /// </summary>
        public DateTime? Date_ { get; set; }
        /// <summary>
        /// Açıklama
        /// </summary>
        public string Description_ { get; set; }
        /// <summary>
        /// Uygun Olmayan Miktar
        /// </summary>
        public decimal UnsuitableAmount { get; set; }
        /// <summary>
        /// Uygunsuzluk İş Emri Oluşacak
        /// </summary>
        public bool IsUnsuitabilityWorkOrder { get; set; }
        /// <summary>
        /// Red
        /// </summary>
        public bool IsReject { get; set; }
        /// <summary>
        /// Düzeltme
        /// </summary>
        public bool IsCorrection { get; set; }
        /// <summary>
        /// Olduğu Gibi Kullanılacak
        /// </summary>
        public bool IsToBeUsedAs { get; set; }
        /// <summary>
        /// Tedarikçi ile İrtibat
        /// </summary>
        public bool IsContactSupplier { get; set; }
        /// <summary>
        /// Sipariş ID
        /// </summary>
        public Guid? OrderID { get; set; }
        /// <summary>
        /// Stok ID
        /// </summary>
        public Guid? ProductID { get; set; }
        /// <summary>
        /// Sipariş ID
        /// </summary>
        public Guid? CurrentAccountCardID { get; set; }
    }
}
