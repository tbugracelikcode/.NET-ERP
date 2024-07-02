using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;
using TsiErp.Entities.Enums;

namespace TsiErp.Entities.Entities.ProductionManagement.ProductionOrder.Dtos
{
    public class ListProductionOrdersDto : FullAuditedEntityDto
    {
        /// <summary>
        /// Üretim Emri Fiş No
        /// </summary>
        public string FicheNo { get; set; }
        /// <summary>
        /// Tarih
        /// </summary>
        public DateTime? Date_ { get; set; }
        /// <summary>
        /// İptal
        /// </summary>
        public bool Cancel_ { get; set; }
        /// <summary>
        /// Üretim Emri Durumu
        /// </summary>
        public ProductionOrderStateEnum ProductionOrderState { get; set; }

        /// <summary>
        /// Planlanan Miktar
        /// </summary>
        public decimal PlannedQuantity { get; set; }

        /// <summary>
        /// Üretilen Miktar
        /// </summary>
        public decimal ProducedQuantity { get; set; }
        /// <summary>
        /// Açıklama
        /// </summary>
        public string Description_ { get; set; }
        /// <summary>
        /// Müşteri Sipariş No
        /// </summary>
        public string CustomerOrderNo { get; set; }
        /// <summary>
        /// Sipariş ID
        /// </summary>
        public Guid? OrderID { get; set; }

        /// <summary>
        /// Sipariş Fiş No
        /// </summary>
        public string OrderFicheNo { get; set; }


        /// <summary>
        /// Mamül ID
        /// </summary>
        public Guid? FinishedProductID { get; set; }

        /// <summary>
        /// Mamül Kodu
        /// </summary>
        public string FinishedProductCode { get; set; }
        /// <summary>
        /// Mamül Açıklaması
        /// </summary>
        public string FinishedProductName { get; set; }

        /// <summary>
        /// Bağlı Ürün Kodu
        /// </summary>
        public string LinkedProductCode { get; set; }
        /// <summary>
        /// Bağlı Ürün Açıklaması
        /// </summary>
        public string LinkedProductName { get; set; }

        /// <summary>
        /// Birim Seti Kodu
        /// </summary>
        public string UnitSetCode { get; set; }

        /// <summary>
        /// Reçete Kodu
        /// </summary>
        public string BOMCode { get; set; }
        /// <summary>
        /// Reçete Açıklaması
        /// </summary>
        public string BOMName { get; set; }

        /// <summary>
        /// Rota Kodu
        /// </summary>
        public string RouteCode { get; set; }
        /// <summary>
        /// Rota Açıklaması
        /// </summary>
        public string RouteName { get; set; }

        /// <summary>
        /// Ürün Ağacı Kodu
        /// </summary>
        public string ProductTreeCode { get; set; }
        /// <summary>
        /// Ürün Ağacı Açıklaması
        /// </summary>
        public string ProductTreeName { get; set; }

        /// <summary>
        /// Teklif Fiş No
        /// </summary>
        public string PropositionFicheNo { get; set; }
        /// <summary>
        /// Cari Hesap ID
        /// </summary>
        public Guid? CurrentAccountID { get; set; }

        /// <summary>
        /// Cari Hesap Kodu
        /// </summary>
        public string CurrentAccountCode { get; set; }
        /// <summary>
        /// Cari Hesap Ünvanı
        /// </summary>
        public string CurrentAccountName { get; set; }
        /// <summary>
        /// Müşteri Kodu
        /// </summary>
        public string CustomerCode { get; set; }
        /// <summary>
        /// Bağlı Üretim Emri ID
        /// </summary>
        public Guid? LinkedProductionOrderID { get; set; }

        /// <summary>
        /// Bağlı Üretim Emri Fiş No
        /// </summary>
        public string LinkedProductionOrderFicheNo { get; set; }
        /// <summary>
        /// Şube ID
        /// </summary>
        public Guid? BranchID { get; set; }
        /// <summary>
        /// Şube Kodu
        /// </summary>
        public string BranchCode { get; set; }


        /// <summary>
        /// Depo ID
        /// </summary>
        public Guid? WarehouseID { get; set; }
        /// <summary>
        /// Depo Kodu
        /// </summary>
        public string WarehouseCode { get; set; }
    }
}
