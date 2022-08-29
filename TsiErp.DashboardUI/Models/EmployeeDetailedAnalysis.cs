using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TsiErp.DashboardUI.Models
{
    public class EmployeeDetailedAnalysis
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
        public string ProductCode { get; set; }
        /// <summary>
        /// Planlanan Miktar 
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
        /// Planlanan Süre
        /// </summary>
        public decimal PlannedProductionTime { get; set; }
        /// <summary>
        /// Üretim Süresi
        /// </summary>
        public decimal OccuredProductionTime { get; set; }
        /// <summary>
        /// Planlanan Birim Süre
        /// </summary>
        public decimal PlannedUnitTime { get; set; }
        /// <summary>
        /// Gerçekleşen Birim Süre
        /// </summary>
        public decimal OccuredUnitTime { get; set; }

    }
}
