using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TsiErp.DashboardUI.Models
{
    public class StationDetailedHaltAnalysisAll
    {
        /// <summary>
        /// Duruş ID 
        /// </summary>
        public int HaltID { get; set; }
        /// <summary>
        /// Duruş Kodu 
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Süre
        /// </summary>
        public int Time { get; set; }
        /// <summary>
        /// İstasyon Adı
        /// </summary>
        public string StationName { get; set; }
        /// <summary>
        /// Yüzde
        /// </summary>
        public double? Percent { get; set; }
        /// <summary>
        /// Toplam
        /// </summary>
        public int Total { get; set; }
    }
}
