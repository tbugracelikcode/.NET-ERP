using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;
using TsiErp.Entities.Entities.Currency;
using TsiErp.Entities.Entities.Employee;
using TsiErp.Entities.Entities.SalesPriceLine;

namespace TsiErp.Entities.Entities.SalesPrice
{
    public class SalesPrices : FullAuditedEntity
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
        /// Başlangıç Tarihi
        /// </summary>
        public DateTime? StartDate { get; set; }
        /// <summary>
        /// Bitiş Tarihi
        /// </summary>
        public DateTime? EndDate { get; set; }
        /// <summary>
        /// Para Birimi ID
        /// </summary>
        public Guid CurrencyID { get; set; }

        public Currencies Currencies { get; set; }
        public ICollection<SalesPriceLines> SalesPriceLines { get; set; }
    }
}
