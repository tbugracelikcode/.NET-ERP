﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;
using Tsi.Core.Utilities.SqlDataTypeMappingUtilities;

namespace TsiErp.Entities.Entities.SalesManagement.OrderAcceptanceRecordLine.Dtos
{
    public class CreateOrderAcceptanceRecordLinesDto : FullAuditedEntityDto
    {
        /// <summary>
        /// Sipariş Kabul ID
        /// </summary>
        public Guid OrderAcceptanceRecordID { get; set; }
        /// <summary>
        /// Satır No
        /// </summary>
        public int LineNr { get; set; }
        /// <summary>
        /// Satın Alma Temin Tarihi
        /// </summary>
        public DateTime? PurchaseSupplyDate { get; set; }
        /// <summary>
        /// Stok ID
        /// </summary>
        public Guid? ProductID { get; set; }
        /// <summary>
        /// Ürün Referans No ID
        /// </summary>
        public Guid? ProductReferanceNumberID { get; set; }
        /// <summary>
        /// Sipariş Referans No
        /// </summary>
        public string OrderReferanceNo { get; set; }
        /// <summary>
        /// Müşteri Referans No
        /// </summary>
        public string CustomerReferanceNo { get; set; }
        /// <summary>
        /// Müşteri Barkod No
        /// </summary>
        public string CustomerBarcodeNo { get; set; }
        /// <summary>
        /// Minimum Sipariş Miktarı
        /// </summary>
        public decimal MinOrderAmount { get; set; }
        /// <summary>
        /// Sipariş Miktarı
        /// </summary>
        public decimal OrderAmount { get; set; }
        /// <summary>
        /// Birim Seti ID
        /// </summary>
        public Guid? UnitSetID { get; set; }
        /// <summary>
        /// Tanımlı Birim Fiyat
        /// </summary>
        public decimal DefinedUnitPrice { get; set; }
        /// <summary>
        /// Sipariş Birim Fiyat
        /// </summary>
        public decimal OrderUnitPrice { get; set; }
        /// <summary>
        /// Satır Tutarı
        /// </summary>
        public decimal LineAmount { get; set; }
        /// <summary>
        /// Açıklama
        /// </summary>
        public string Description_ { get; set; }
        /// <summary>
        /// Stok Kodu
        /// </summary>
        public string ProductCode { get; set; }
        /// <summary>
        /// Ödeme Planı ID
        /// </summary>
        public Guid PaymentPlanID { get; set; }
    }
}
