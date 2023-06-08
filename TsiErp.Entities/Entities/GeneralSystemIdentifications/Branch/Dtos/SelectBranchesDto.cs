using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.GeneralSystemIdentifications.Branch.Dtos
{
    public class SelectBranchesDto : FullAuditedEntityDto
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
        /// Data Concurrency Stamp
        /// </summary>
        public bool? DataOpenStatus { get; set; }

        /// <summary>
        /// Data Concurrency UserID
        /// </summary>
        public Guid? DataOpenStatusUserId { get; set; }
    }
}
