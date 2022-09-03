using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TsiErp.DashboardUI.Models
{
    public class StationDetailedProductChart
    {
        /// <summary>
        /// Stok ID 
        /// </summary>
        public int ProductID { get; set; }
        /// <summary>
        /// Stok Açıklaması
        /// </summary>
        public string ProductGroup { get; set; }
        /// <summary>
        /// Performans
        /// </summary>
        public decimal Performance { get; set; }
    }
}
