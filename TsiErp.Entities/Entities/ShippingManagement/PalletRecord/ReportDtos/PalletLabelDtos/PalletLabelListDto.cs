using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TsiErp.Entities.Entities.ShippingManagement.PalletRecord.ReportDtos.PalletLabelDtos
{
    public class PalletLabelListDto
    {
        /// <summary>
        /// Müşteri referans no
        /// </summary>
        public string CustomerReferenceNo { get; set; }

        /// <summary>
        /// Stok ID
        /// </summary>
        public Guid ProductId { get; set; }


        /// <summary>
        /// Tedarikçi No
        /// </summary>
        public string SupplierNo { get; set; }

        /// <summary>
        /// Çeki Listesi No
        /// </summary>
        public string PackingListNo { get; set; }

        /// <summary>
        /// Sevkiyat Adresi
        /// </summary>
        public string ShippingAddress { get; set; }

        /// <summary>
        /// AREKS Stok Kodu
        /// </summary>
        public string AreksProductCode { get; set; }

        /// <summary>
        /// İngilizce Tanım
        /// </summary>
        public string EnglishDefinition { get; set; }

        /// <summary>
        /// Paket No.
        /// </summary>
        public string PackageNo { get; set; }

        /// <summary>
        /// Toplam Adet
        /// </summary>
        public decimal TotalQuantity { get; set; }

        /// <summary>
        /// Birim Seti
        /// </summary>
        public string UnitSet { get; set; }

        /// <summary>
        /// Paket Adeti
        /// </summary>
        public int PackageQuantity { get; set; }

        /// <summary>
        /// Palet No
        /// </summary>
        public string PalletNo { get; set; }

        /// <summary>
        /// Referans No
        /// </summary>
        public string ReferenceNo { get; set; }
    }
}
