using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TsiErp.Entities.Entities.CurrentAccountCard;
using TsiErp.Entities.Entities.ExchangeRate;
//using TsiErp.Entities.Entities.SalesProposition;
using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.Currency
{
    /// <summary>
    /// Para Birimleri
    /// </summary>
    public class Currencies : FullAuditedEntity
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

        public ICollection<CurrentAccountCards> CurrentAccountCards { get; set; }

        //public ICollection<SalesPropositions> SalesPropositions { get; set; }

        public ICollection<ExchangeRates> ExchangeRates { get; set; }
    }
}
