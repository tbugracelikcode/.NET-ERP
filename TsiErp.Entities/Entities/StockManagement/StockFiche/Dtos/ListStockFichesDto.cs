using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;
using TsiErp.Entities.Enums;

namespace TsiErp.Entities.Entities.StockManagement.StockFiche.Dtos
{
    public class ListStockFichesDto : FullAuditedEntityDto
    {
        /// <summary>
        /// Satış Fiş No
        /// </summary>
        public string FicheNo { get; set; }
        /// <summary>
        /// Üretim Tarihi Referansı
        /// </summary>
        public string ProductionDateReferance { get; set; }
        /// <summary>
        /// Tarih
        /// </summary>
        public DateTime Date_ { get; set; }
        /// <summary>
        /// Açıklama
        /// </summary>
        public string Description_ { get; set; }


        /// <summary>
        /// Satın Alma Sipariş ID
        /// </summary>
        public Guid? PurchaseOrderID { get; set; }

        /// <summary>
        /// Satın Alma Sipariş Fiş No
        /// </summary>
        public string PurchaseOrderFicheNo { get; set; }
        /// <summary>
        /// Satın Alma Talep ID
        /// </summary>
        public Guid? PurchaseRequestID { get; set; }
        /// <summary>
        /// Satın Alma Talep Fiş No
        /// </summary>
        public string PurchaseRequestFicheNo { get; set; }
        /// <summary>
        /// Fiş Türü
        /// </summary>
        public StockFicheTypeEnum FicheType { get; set; }
        /// <summary>
        /// Giriş Çıkış Kodu
        /// </summary>
        public int InputOutputCode { get; set; }
        /// <summary>
        /// Şube Kodu
        /// </summary>
        public string BranchCode { get; set; }
        /// <summary>
        /// Depo Kodu
        /// </summary>
        public string WarehouseCode { get; set; }
        /// <summary>
        /// Net Tutar
        /// </summary>
        public decimal NetAmount { get; set; }

        /// <summary>
        /// İşlem Dövizi ID
        /// </summary>
        public Guid? TransactionExchangeCurrencyID { get; set; }
        /// <summary>
        /// İşlem Dövizi Kodu
        /// </summary>
        public string TransactionExchangeCurrencyCode { get; set; }
    }
}
