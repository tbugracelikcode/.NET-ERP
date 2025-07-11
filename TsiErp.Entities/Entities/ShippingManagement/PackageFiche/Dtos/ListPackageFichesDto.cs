﻿using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.ShippingManagement.PackageFiche.Dtos
{
    public class ListPackageFichesDto : FullAuditedEntityDto
    {

        /// <summary>
        /// Paket Fişi Kodu
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Çeki Listesi ID
        /// </summary>
        public Guid? PackingListID { get; set; }
        /// <summary>
        /// Paket Fişi Tarihi
        /// </summary>
        public DateTime? Date_ { get; set; }
        /// <summary>
        /// Üretim Emri ID
        /// </summary>
        public Guid? ProductionOrderID { get; set; }
        /// <summary>
        /// Üretim Emri Referans No
        /// </summary>
        public string ProductionOrderFicheNo { get; set; }
        /// <summary>
        /// Üretim Emri Referans No
        /// </summary>
        public string ProductionOrderReferenceNo { get; set; }
        /// <summary>
        /// Satış Sipariş ID
        /// </summary>
        public Guid? SalesOrderID { get; set; }
        /// <summary>
        /// Satış Sipariş Fiş No
        /// </summary>
        public string SalesOrderFicheNo { get; set; }
        /// <summary>
        /// Müşteri Sipariş No
        /// </summary>
        public string SalesOrderCustomerOrderNo { get; set; }
        /// <summary>
        /// Stok ID
        /// </summary>
        public Guid? ProductID { get; set; }
        /// <summary>
        /// Stok Kodu
        /// </summary>
        public string ProductCode { get; set; }
        /// <summary>
        /// Stok Adı
        /// </summary>
        public string ProductName { get; set; }
        /// <summary>
        /// Tanımlı Birim Ağırlık
        /// </summary>
        public decimal ProductUnitWeight { get; set; }
        /// <summary>
        /// Cari Hesap ID
        /// </summary>
        public Guid? CurrentAccountID { get; set; }
        /// <summary>
        /// Varyant Kodu
        /// </summary>
        public string CustomerCode { get; set; }
        /// <summary>
        /// Koli Türü
        /// </summary>
        public string PackageType { get; set; }
        /// <summary>
        /// Koli İçeriği
        /// </summary>
        public int PackageContent { get; set; }
        /// <summary>
        /// Koli Sayısı
        /// </summary>
        public int NumberofPackage { get; set; }
        /// <summary>
        /// Palet No
        /// </summary>
        public string PalletNumber { get; set; }
        /// <summary>
        /// Ürün Palet Sırası
        /// </summary>
        public string ProductPalletOrder { get; set; }
        /// <summary>
        /// Birim Ağırlık
        /// </summary>
        public decimal UnitWeight { get; set; }

    }
}
