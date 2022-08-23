using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TsiErp.Dashboard.Models
{
    public class StationDetailedProductAnalysis
    {
        /// <summary>
        /// İş Emri ID 
        /// </summary>
        public int WorkOrderID { get; set; }
        /// <summary>
        /// Stok ID 
        /// </summary>
        public int ProductID { get; set; }
        /// <summary>
        /// Stok Kodu
        /// </summary>
        public string ProductCode { get; set; }
        /// <summary>
        /// Stok Açıklaması
        /// </summary>
        public string ProductGroup { get; set; }
        /// <summary>
        /// Üretilen Adet 
        /// </summary>
        public int TotalProduction { get; set; }
        /// <summary>
        /// Hurda Adet
        /// </summary>
        public int TotalScrap { get; set; }
        /// <summary>
        /// Planlanan Birim Süre 
        /// </summary>
        public int PlannedUnitTime { get; set; }
        /// <summary>
        /// Gerçekleşen Birim Süre 
        /// </summary>
        public int OccuredUnitTime { get; set; }
        /// <summary>
        /// Planlanan Adet
        /// </summary>
        public int PlannedQuantity { get; set; }
        /// <summary>
        /// Performans
        /// </summary>
        public decimal Performance { get; set; }
    }
}
