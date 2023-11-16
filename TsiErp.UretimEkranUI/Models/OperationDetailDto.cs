using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TsiErp.Entities.Enums;

namespace TsiErp.UretimEkranUI.Models
{
    public class OperationDetailDto
    {

        /// <summary>
        /// İş Emri No
        /// </summary>
        public string WorkOrderNo { get; set; }

        /// <summary>
        /// Durum
        /// </summary>
        public WorkOrderStateEnum WorkOrderState { get; set; }

        /// <summary>
        /// Planlanan Miktar
        /// </summary>
        public decimal PlannedQuantity { get; set; }

        /// <summary>
        /// Gerçekleşen Miktar
        /// </summary>
        public decimal ProducedQuantity { get; set; }

        /// <summary>
        /// Üretim Emri ID
        /// </summary>
        public Guid? ProductionOrderID { get; set; }

        /// <summary>
        /// Üretim Emri Fiş No
        /// </summary>
        public string ProductionOrderFicheNo { get; set; }

        /// <summary>
        /// Ürüne Özel Operasyon ID
        /// </summary>
        public Guid? ProductsOperationID { get; set; }

        /// <summary>
        /// Ürüne Özel Operasyon Kodu
        /// </summary>
        public string ProductsOperationCode { get; set; }

        /// <summary>
        /// Ürüne Özel Operasyon Açıklaması
        /// </summary>
        public string ProductsOperationName { get; set; }

        /// <summary>
        /// İş İstasyonu ID
        /// </summary>
        public Guid? StationID { get; set; }

        /// <summary>
        /// İstasyon Kodu
        /// </summary>
        public string StationCode { get; set; }

        /// <summary>
        /// İstasyon Açıklaması
        /// </summary>
        public string StationName { get; set; }

        /// <summary>
        /// Stok ID
        /// </summary>
        public Guid? ProductID { get; set; }

        /// <summary>
        /// Stok Kodu
        /// </summary>
        public string ProductCode { get; set; }

        /// <summary>
        /// Stok Açıklaması
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// Personel Id
        /// </summary>
        public Guid EmployeeID { get; set; }

        /// <summary>
        /// Personel Adı
        /// </summary>
        public string EmployeeName { get; set; }


        public OperationAdjustmentDto OperationAdjustment { get; set; }


        public OperationDetailDto()
        {
            OperationAdjustment = new OperationAdjustmentDto();
        }
    }
}
