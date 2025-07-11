﻿using System;
using System.Collections.Generic;
using System.Text;
using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.GeneralSystemIdentifications.Currency.Dtos
{
    public class UpdateCurrenciesDto : FullAuditedEntityDto
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
        /// Yerel Para Birimi
        /// </summary>
        public bool IsLocalCurrency { get; set; }

        /// <summary>
        /// Sembol
        /// </summary>
        public string CurrencySymbol { get; set; }
    }
}
