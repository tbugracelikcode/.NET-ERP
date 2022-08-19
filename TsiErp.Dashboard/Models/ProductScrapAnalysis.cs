using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TsiErp.Dashboard.Models
{
    public class ProductScrapAnalysis
    {
        /// <summary>
        /// Sebep ID 
        /// </summary>
        public int ScrapCauseID { get; set; }
        /// <summary>
        /// Sebep
        /// </summary>
        public string ScrapCauseName { get; set; }
        /// <summary>
        /// Hurda Adet
        /// </summary>
        public int TotalScrap { get; set; }
        /// <summary>
        /// Üretilen Adet 
        /// </summary>
        public int TotalProduction { get; set; }
        /// <summary>
        /// PPM
        /// </summary>
        public decimal PPM { get; set; }
    }
}
