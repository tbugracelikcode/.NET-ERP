using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TsiErp.DashboardUI.Models
{
    public class StationDetailedEmployeeAnalysis
    {
        /// <summary>
        /// Personel ID 
        /// </summary>
        public int EmployeeID { get; set; }
        /// <summary>
        /// Personel Adı
        /// </summary>
        public string EmployeeName { get; set; }
        /// <summary>
        /// Üretilen Adet
        /// </summary>
        public int TotalProduction { get; set; }
        /// <summary>
        /// Hurda Adet
        /// </summary>
        public int TotalScrap { get; set; }
        /// <summary>
        /// Çalışma Süresi
        /// </summary>
        public int OperationTime { get; set; }
        /// <summary>
        /// Toplam Çalışma Süresi
        /// </summary>
        public int TotalTime { get; set; }
        /// <summary>
        /// Toplam Çalışma Süresi
        /// </summary>
        public double Percent { get; set; }
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
    }
}
