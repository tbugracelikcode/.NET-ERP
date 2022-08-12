using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Domain.Entities.Auditing;
using TsiErp.Entities.Entities.Period;

namespace TsiErp.Entities.Entities.Branch
{
    public class Branches : FullAuditedEntity
    {
        /// <summary>
        /// Kod
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// İsim
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Açıklama
        /// </summary>
        public string Description_ { get; set; }
        /// <summary>
        /// Aktiflik
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Dönemler
        /// </summary>
        public ICollection<Periods> Periods { get; set; }
    }
}
