using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TsiErp.Entities.Entities.ShippingManagement.PalletRecord.ReportDtos.PalletLabelDtos
{
    public class PalletLabelDto
    {

        /// <summary>
        /// Areks Çeki Listesi No
        /// </summary>
        public string AreksPackingNo { get; set; }

        /// <summary>
        /// Çeki Listesi No
        /// </summary>
        public string PackingListNo { get; set; }

        /// <summary>
        /// Fatura No
        /// </summary>
        public string BillNo { get; set; }

        /// <summary>
        /// Tedarikçi No
        /// </summary>
        public string SupplierNo { get; set; }

        /// <summary>
        /// Gönderici Şirket
        /// </summary>
        public string SenderCompany { get; set; }

        /// <summary>
        /// Sevkiyat Adresi
        /// </summary>
        public string ShippingAddress { get; set; }

        /// <summary>
        /// Gönderilen Şirket
        /// </summary>
        public string SentCompany { get; set; }

        /// <summary>
        /// Adet
        /// </summary>
        public decimal Quantity { get; set; }

        /// <summary>
        /// Net KG.
        /// </summary>
        public decimal NetKg { get; set; }

        /// <summary>
        /// Brüt KG.
        /// </summary>
        public decimal GrossKg { get; set; }

        /// <summary>
        /// Markalama Referans No.
        /// </summary>
        public string MarkingRefNo { get; set; }

        /// <summary>
        /// Palet No.
        /// </summary>
        public string PalletNo { get; set; }

        /// <summary>
        /// Sipariş No.
        /// </summary>
        public string OrderNo { get; set; }

        /// <summary>
        /// Teslimat Tarihi
        /// </summary>
        public string DeliveryDate { get; set; }
    }
}
