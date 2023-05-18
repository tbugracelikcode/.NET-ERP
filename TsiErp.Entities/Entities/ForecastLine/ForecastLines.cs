using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;
using TsiErp.Entities.Entities.Forecast;
using TsiErp.Entities.Entities.Product;

namespace TsiErp.Entities.Entities.ForecastLine
{
    public class ForecastLines : FullAuditedEntity
    {
        /// <summary>
        /// Forecast ID
        /// </summary>
        public Guid ForecastID { get; set; }
        /// <summary>
        /// Stok ID
        /// </summary>
        public Guid ProductID { get; set; }
        [Precision(18, 6)]
        /// <summary>
        /// Miktar
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// Müşteri Stok Kodu
        /// </summary>
        public string CustomerProductCode { get; set; }
        /// <summary>
        /// Satır Nr
        /// </summary>
        public int LineNr { get; set; }
    }
}
