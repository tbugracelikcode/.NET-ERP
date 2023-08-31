using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.QualityControl.PurchaseUnsuitabilityReport.Dtos
{
    public class ListPurchaseUnsuitabilityReportsDto : FullAuditedEntityDto
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
        /// Hata Başlığı ID
        /// </summary>
        public Guid UnsuitabilityItemsID { get; set; }
        /// <summary>
        /// Aksiyon
        /// </summary>
        public string Action_ { get; set; }
        /// <summary>
        /// Sipariş Fiş No
        /// </summary>
        public string OrderFicheNo { get; set; }
        /// <summary>
        /// Stok Kodu
        /// </summary>
        public string ProductCode { get; set; }
        /// <summary>
        /// Stok Açıklaması
        /// </summary>
        public string ProductName { get; set; }
        /// <summary>
        /// Cari Kod
        /// </summary>
        public string CurrentAccountCardCode { get; set; }
        /// <summary>
        /// Cari Ünvan
        /// </summary>
        public string CurrentAccountCardName { get; set; }
    }
}
