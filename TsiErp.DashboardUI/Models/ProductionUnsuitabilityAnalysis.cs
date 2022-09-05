using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TsiErp.DashboardUI.Models
{
    public class ProductionUnsuitabilityAnalysis
    {
        /// <summary>
        /// Üretim Uygunsuzluk ID 
        /// </summary>
        public int ProductionUnsuitabilityID { get; set; }
        /// <summary>
        /// Hurda Adet 
        /// </summary>
        public int ScrapQuantity { get; set; }
        /// <summary>
        /// Uygunsuzluk Sebebi 
        /// </summary>
        public string UnsuitabilityReason { get; set; }
        /// <summary>
        /// Düzeltme
        /// </summary>
        public int Correction { get; set; }
        /// <summary>
        /// Olduğu Gibi Kullanılacak
        /// </summary>
        public int ToBeUsedAs { get; set; }
        /// <summary>
        /// Toplam 
        /// </summary>
        public int Total { get; set; }
        /// <summary>
        /// Kod 
        /// </summary>
        public string Code { get; set; }
    }
}
