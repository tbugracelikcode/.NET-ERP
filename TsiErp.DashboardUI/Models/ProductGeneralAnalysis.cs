using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TsiErp.DashboardUI.Models
{
    public class ProductGeneralAnalysis
    {
        /// <summary>
        /// İş Emri ID 
        /// </summary>
        public int WorkOrderID { get; set; }
        /// <summary>
        /// İş Emri No 
        /// </summary>
        public string WorkOrderNr { get; set; }
        /// <summary>
        /// Stok ID 
        /// </summary>
        public int ProductID { get; set; }
        /// <summary>
        /// Stok Kodu 
        /// </summary>
        public string ProductName { get; set; }
        /// <summary>
        /// Planlanan Adet 
        /// </summary>
        public decimal PlannedQuantity { get; set; }
        /// <summary>
        /// Üretilen Adet 
        /// </summary>
        public decimal TotalProduction { get; set; }
        /// <summary>
        /// Hurda Adet 
        /// </summary>
        public decimal TotalScrap { get; set; }
        /// <summary>
        /// Üretim Süresi
        /// </summary>
        public decimal OperationTime { get; set; }
        /// <summary>
        /// Ayar Süresi
        /// </summary>
        public decimal AdjustmentTime { get; set; }
        /// <summary>
        /// Operasyon ID
        /// </summary>
        public int OperationID { get; set; }
        /// <summary>
        /// Operasyon Adı
        /// </summary>
        public string OperationName { get; set; }
        /// <summary>
        /// Birim Süre
        /// </summary>
        public decimal UnitTime { get; set; }
    }
}
