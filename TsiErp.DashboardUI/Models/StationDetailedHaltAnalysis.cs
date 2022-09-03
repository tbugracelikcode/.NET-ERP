using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TsiErp.DashboardUI.Models
{
    public class StationDetailedHaltAnalysis
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
    }
}
