using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;
using TsiErp.Entities.Entities.Product;
//using TsiErp.Entities.Entities.SalesPropositionLine;

namespace TsiErp.Entities.Entities.UnitSet
{
    /// <summary>
    /// Birim Setleri
    /// </summary>
    public class UnitSets : FullAuditedEntity
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
        /// <summary>
        /// Stoklar
        /// </summary>
        ///  /// <summary>
        /// Stoklar
        /// </summary>
        public ICollection<Products> Products { get; set; }
        ///// <summary>
        ///// Satış Teklifleri
        ///// </summary>
        //public ICollection<SalesPropositionLines> SalesPropositionLines { get; set; }
    }
}
