using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;
using TsiErp.Entities.Enums;

namespace TsiErp.Entities.Entities.ProductionManagement.ProductionOrder.Dtos
{
    public class UpdateProductionOrdersDto : FullAuditedEntityDto
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
        public int ProductionOrderState { get; set; }
        /// <summary>
        /// Şube ID
        /// </summary>
        public Guid? BranchID { get; set; }


        /// <summary>
        /// Depo ID
        /// </summary>
        public Guid? WarehouseID { get; set; }

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
        /// Sipariş Satır ID
        /// </summary>
        public Guid? OrderLineID { get; set; }
        /// <summary>
        /// Mamül ID
        /// </summary>
        public Guid? FinishedProductID { get; set; }
        /// <summary>
        /// Bağlı Ürün ID
        /// </summary>
        public Guid? LinkedProductID { get; set; }
        /// <summary>
        /// Birim Seti
        /// </summary>
        public Guid? UnitSetID { get; set; }
        /// <summary>
        /// Reçete ID
        /// </summary>
        public Guid? BOMID { get; set; }
        /// <summary>
        /// Rota ID
        /// </summary>
        public Guid? RouteID { get; set; }
        /// <summary>
        /// Ürün Ağacı ID
        /// </summary>
        public Guid? ProductTreeID { get; set; }
        /// <summary>
        /// Ürün Ağacı Satır ID
        /// </summary>
        public Guid? ProductTreeLineID { get; set; }
        /// <summary>
        /// Teklif ID
        /// </summary>
        public Guid? PropositionID { get; set; }
        /// <summary>
        /// Teklif Satır ID
        /// </summary>
        public Guid? PropositionLineID { get; set; }
        /// <summary>
        /// Cari Hesap ID
        /// </summary>
        public Guid? CurrentAccountID { get; set; }
        /// <summary>
        /// Bağlı Üretim Emri ID
        /// </summary>
        public Guid? LinkedProductionOrderID { get; set; }
    }
}
