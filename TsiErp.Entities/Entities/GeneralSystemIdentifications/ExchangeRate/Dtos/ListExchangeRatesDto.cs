using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.GeneralSystemIdentifications.ExchangeRate.Dtos
{
    public class ListExchangeRatesDto : FullAuditedEntityDto
    {
        /// <summary>
        /// Parabirimi Id
        /// </summary>
        public Guid CurrencyId { get; set; }

        /// <summary>
        /// Parabirimi kodu
        /// </summary>
        public string CurrencyCode { get; set; }

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
