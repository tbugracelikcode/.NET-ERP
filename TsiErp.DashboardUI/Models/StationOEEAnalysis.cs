using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TsiErp.DashboardUI.Models
{
    public class StationOEEAnalysis
    {
        /// <summary>
        /// Makina ID 
        /// </summary>
        public int StationID { get; set; }
        /// <summary>
        /// Makina Kodu
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Vardiya Süresi
        /// </summary>
        public decimal ShiftTime { get; set; }
        /// <summary>
        /// Planlanan Operasyon Süresi 
        /// </summary>
        public decimal PlannedOperationTime { get; set; }
        /// <summary>
        /// Gerçekleşen Operasyon Süresi 
        /// </summary>
        public decimal OccuredOperationTime { get; set; }
        /// <summary>
        /// Duruş
        /// </summary>
        public decimal HaltTime { get; set; }
        /// <summary>
        /// Hurda Süresi
        /// </summary>
        public decimal ScrapTime { get; set; }
        /// <summary>
        /// Kullanılabilirlik 
        /// </summary>
        public decimal Availability { get; set; }
        /// <summary>
        /// Performans
        /// </summary>
        public decimal Performance { get; set; }
        /// <summary>
        /// Kalite
        /// </summary>
        public decimal Quality { get; set; }
        /// <summary>
        /// OEE
        /// </summary>
        public decimal OEE { get; set; }
        /// <summary>
        /// Tarih
        /// </summary>
        public DateTime Date { get; set; }
        /// <summary>
        /// Departman
        /// </summary>
        public string Department { get; set; }
    }
}
