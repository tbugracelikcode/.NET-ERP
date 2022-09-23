using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.ExchangeRate.Dtos
{
    public class CreateExchangeRatesDto : FullAuditedEntityDto
    {
        /// <summary>
        /// Parabirimi ID
        /// </summary>
        public Guid CurrencyID { get; set; }
        public DateTime Date { get; set; }
        /// <summary>
        /// Alış Kuru
        /// </summary>
        public decimal BuyingRate { get; set; }
        /// <summary>
        /// Satış Kuru
        /// </summary>
        public decimal SaleRate { get; set; }
        /// <summary>
        /// Efektif Alış Kuru
        /// </summary>
        public decimal EffectiveBuyingRate { get; set; }
        /// <summary>
        /// Efektif Satış Kuru
        /// </summary>
        public decimal EffectiveSaleRate { get; set; }
    }
}
