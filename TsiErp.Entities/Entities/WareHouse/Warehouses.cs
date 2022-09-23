using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using TsiErp.Entities.Entities.SalesPropositionLine;
//using TsiErp.Entities.Entities.SalesProposition;
using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.WareHouse
{
    /// <summary>
    /// Depo
    /// </summary>
    public class Warehouses : FullAuditedEntity
    {
        /// <summary>
        /// Kod
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Açıklama
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Aktiflik
        /// </summary>
        public bool IsActive { get; set; }
        ///// <summary>
        ///// Satış Teklifleri 
        ///// </summary>
        //public ICollection<SalesPropositions> SalesPropositions { get; set; }
        ///// <summary>
        ///// Satış Teklif Satırları
        ///// </summary>
        //public ICollection<SalesPropositionLines> SalesPropositionLines { get; set; }
    }
}
