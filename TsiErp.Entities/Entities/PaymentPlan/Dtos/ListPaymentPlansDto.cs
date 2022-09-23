using System;
using System.Collections.Generic;
using System.Text;
using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.PaymentPlan.Dtos
{
    public class ListPaymentPlansDto : FullAuditedEntityDto
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
        /// Günler
        /// </summary>
        public int Days_ { get; set; }
        /// <summary>
        /// Gecikme Vade Farkı
        /// </summary>
        public decimal DelayMaturityDifference { get; set; }
        /// <summary>
        /// Aktiflik
        /// </summary>
        public bool IsActive { get; set; }
    }
}
