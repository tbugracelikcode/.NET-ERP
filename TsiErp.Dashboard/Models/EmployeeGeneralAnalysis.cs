using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TsiErp.Dashboard.Models
{
    public class EmployeeGeneralAnalysis
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
        /// Çalışma Süresi
        /// </summary>
        public decimal OperationTime { get; set; }
        /// <summary>
        /// Duruş Süresi
        /// </summary>
        public decimal HaltTime { get; set; }
        /// <summary>
        /// Üretim Adet 
        /// </summary>
        public decimal TotalProduction { get; set; }
        /// <summary>
        /// Hurda Adet 
        /// </summary>
        public decimal TotalScrap { get; set; }
    }
}
