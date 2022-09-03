using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TsiErp.DashboardUI.Models
{
    public class StationGeneralAnalysis
    {
        /// <summary>
        /// Makine ID 
        /// </summary>
        public int StationID { get; set; }
        /// <summary>
        /// Makine Kodu 
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Teorik Süre
        /// </summary>
        public decimal TheoreticalTime { get; set; }
        /// <summary>
        /// Kullanılabilir Zaman 
        /// </summary>
        public decimal AvailableTime { get; set; }
        /// <summary>
        /// Makinanın Açık Olduğu Süre
        /// </summary>
        public decimal StationOnTime { get; set; }
        /// <summary>
        /// Makinanın Üretim Süresi 
        /// </summary>
        public decimal StationProductionTime { get; set; }
        /// <summary>
        /// Makinanın Duruş Süresi 
        /// </summary>
        public decimal StationHaltTime { get; set; }
        /// <summary>
        /// Plansız Duruş 
        /// </summary>
        public int UnplannedHaltTime { get; set; }
        /// <summary>
        /// Planlı Duruş
        /// </summary>
        public int PlannedHaltTime { get; set; }
        /// <summary>
        /// Plansız Oranı
        /// </summary>
        public double UnplannedPercentage { get; set; }
    }
}
