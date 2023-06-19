using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;
using TsiErp.Entities.Entities.StockManagement.StockFicheLine.Dtos;
using TsiErp.Entities.Enums;

namespace TsiErp.Entities.Entities.StockManagement.StockFiche.Dtos
{
    public class SelectStockFichesDto : FullAuditedEntityDto
    {
        /// <summary>
        /// Satış Fiş No
        /// </summary>
        public string FicheNo { get; set; }
        /// <summary>
        /// Data Concurrency Stamp
        /// </summary>
        public bool? DataOpenStatus { get; set; }

        /// <summary>
        /// Data Concurrency UserID
        /// </summary>
        public Guid? DataOpenStatusUserId { get; set; }
        /// <summary>
        /// Tarih
        /// </summary>
        public DateTime Date_ { get; set; }
        /// <summary>
        /// Saat
        /// </summary>
        public TimeSpan? Time_ { get; set; }
        /// <summary>
        /// Açıklama
        /// </summary>
        public string Description_ { get; set; }
        /// <summary>
        /// Özel Kod
        /// </summary>
        public string SpecialCode { get; set; }
        /// <summary>
        /// Fiş Türü
        /// </summary>
        public StockFicheTypeEnum FicheType { get; set; }
        public string FicheTypeName { get; set; }
        /// <summary>
        /// Şube ID
        /// </summary>
        public Guid BranchID { get; set; }
        /// <summary>
        /// Şube Kodu
        /// </summary>
        public string BranchCode { get; set; }
        /// <summary>
        /// Depo ID
        /// </summary>
        public Guid WarehouseID { get; set; }
        /// <summary>
        /// Depo Kodu
        /// </summary>
        public string WarehouseCode { get; set; }
        /// <summary>
        /// Çıkış Depo ID
        /// </summary>
        public Guid TransmitterWarehouseID { get; set; }
        /// <summary>
        /// Çıkış Depo Kodu
        /// </summary>
        public string TransmitterWarehouseCode { get; set; }
        /// <summary>
        /// Üretim Emri ID
        /// </summary>
        public Guid ProductionOrderID { get; set; }
        /// <summary>
        /// Üretim Emri ID
        /// </summary>
        public string ProductionOrderCode { get; set; }
        /// <summary>
        /// Para Birimi ID
        /// </summary>
        public Guid CurrencyID { get; set; }
        /// <summary>
        /// Para Birimi Kodu
        /// </summary>
        public string CurrencyCode { get; set; }

        /// <summary>
        /// Kur Tutarı
        /// </summary>
        public decimal ExchangeRate { get; set; }
        /// <summary>
        /// Net Tutar
        /// </summary>
        public decimal NetAmount { get; set; }


        /// <summary>
        /// Stok Fiş Satırları
        /// </summary>
        public List<SelectStockFicheLinesDto> SelectStockFicheLines { get; set; }
    }
}
