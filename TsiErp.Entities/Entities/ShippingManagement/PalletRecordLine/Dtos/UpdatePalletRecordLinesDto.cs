﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;
using Tsi.Core.Utilities.SqlDataTypeMappingUtilities;

namespace TsiErp.Entities.Entities.ShippingManagement.PalletRecordLine.Dtos
{
    public class UpdatePalletRecordLinesDto : FullAuditedEntityDto
    {
        /// <summary>
        /// Palet Kaydı ID
        /// </summary>
        public Guid PalletRecordID { get; set; }
        /// <summary>
        /// Paket Fiş Satır ID
        /// </summary>
        public Guid PackageFicheLineID { get; set; }
        /// <summary>
        /// Satış Sipariş ID
        /// </summary>
        public Guid SalesOrderID { get; set; }
        /// <summary>
        /// Satır Onay
        /// </summary>
        public bool LineApproval { get; set; }
        /// <summary>
        /// Onaylanan Birim Fiyat
        /// </summary>
        public decimal ApprovedUnitPrice { get; set; }
        /// <summary>
        /// Paket Fiş ID
        /// </summary>
        public Guid? PackageFicheID { get; set; }
        /// <summary>
        /// Stok ID
        /// </summary>
        public Guid? ProductID { get; set; }
        /// <summary>
        /// Cari Hesap ID
        /// </summary>
        public Guid? CurrentAccountCardID { get; set; }
        /// <summary>
        /// Satır No
        /// </summary>
        public int LineNr { get; set; }
        /// <summary>
        /// Koli Türü
        /// </summary>
        public string PackageType { get; set; }
        /// <summary>
        /// Paket İçeriği
        /// </summary>
        public int PackageContent { get; set; }
        /// <summary>
        /// Paket Sayısı
        /// </summary>
        public int NumberofPackage { get; set; }
        /// <summary>
        /// Toplam Adet
        /// </summary>
        public int TotalAmount { get; set; }
        /// <summary>
        /// Toplam Net KG
        /// </summary>
        public decimal TotalNetKG { get; set; }
        /// <summary>
        /// Toplam Brüt KG
        /// </summary>
        public decimal TotalGrossKG { get; set; }
    }
}
