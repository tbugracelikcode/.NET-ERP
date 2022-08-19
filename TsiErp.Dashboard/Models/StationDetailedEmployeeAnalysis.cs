using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TsiErp.Dashboard.Models
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
    }
}
