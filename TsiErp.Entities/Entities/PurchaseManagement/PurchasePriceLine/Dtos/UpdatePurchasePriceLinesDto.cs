﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.PurchaseManagement.PurchasePriceLine.Dtos
{
    public class UpdatePurchasePriceLinesDto : FullAuditedEntityDto
    {
        /// <summary>
        /// Fiyat Liste ID
        /// </summary>
        public Guid PurchasePriceID { get; set; }
        /// <summary>
        /// Stok ID
        /// </summary>
        public Guid? ProductID { get; set; }
        /// <summary>
        /// Para Birimi ID
        /// </summary>
        public Guid? CurrencyID { get; set; }
        /// <summary>
        /// Cari ID
        /// </summary>
        public Guid? CurrentAccountCardID { get; set; }
        /// <summary>
        /// Fiyat
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// Satır No
        /// </summary>
        public int Linenr { get; set; }
        /// <summary>
        /// Başlangıç Tarihi
        /// </summary>
        public DateTime? StartDate { get; set; }
        /// <summary>
        /// Bitiş Tarihi
        /// </summary>
        public DateTime? EndDate { get; set; }
        /// <summary>
        /// Temin Tarih Günü
        /// </summary>
        public int SupplyDateDay { get; set; }
        /// <summary>
        /// Onay
        /// </summary>
        public bool IsApproved { get; set; }
    }
}
