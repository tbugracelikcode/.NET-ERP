using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.GeneralSystemIdentifications.UserGroup.Dtos
{
    public class SelectUserGroupsDto : FullAuditedEntityDto
    {
        /// <summary>
        /// Kullanıcı Grubu Kodu
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Kullanıcı Grubu Adı
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Data Concurrency Stamp
        /// </summary>
        public bool? DataOpenStatus { get; set; }

        /// <summary>
        /// Data Concurrency UserID
        /// </summary>
        public Guid? DataOpenStatusUserId { get; set; }
        /// <summary>
        /// Aktiflik
        /// </summary>
        public bool IsActive { get; set; }
    }
}
