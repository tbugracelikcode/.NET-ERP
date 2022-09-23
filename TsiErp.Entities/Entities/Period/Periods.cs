using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;
using TsiErp.Entities.Entities.Branch;

namespace TsiErp.Entities.Entities.Period
{




    /// <summary>
    /// Dönemler
    /// </summary>
    public class Periods : FullAuditedEntity
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
        /// Şube ID
        /// </summary>
        public Guid BranchID { get; set; }


        public Branches Branches { get; set; }
    }
}
