using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TsiErp.DashboardUI.Models
{
    public class ContractUnsuitabilityAnalysis
    {
        /// <summary>
        /// Fason Uygunsuzluk ID 
        /// </summary>
        public int ContractUnsuitabilityID { get; set; }
        /// <summary>
        /// Red Miktar 
        /// </summary>
        public int RefuseQuantity { get; set; }
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
        /// Hurda Adet
        /// </summary>
        public int ScrapQuantity { get; set; }
        /// <summary>
        /// Toplam 
        /// </summary>
        public int Total { get; set; }
        /// <summary>
        /// Hata ID 
        /// </summary>
        public int ErrorID { get; set; }
    }
}
