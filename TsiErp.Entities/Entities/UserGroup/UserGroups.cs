using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;
using TsiErp.Entities.Entities.User;

namespace TsiErp.Entities.Entities.UserGroup
{
    /// <summary>
    /// Kullanıcı Grupları
    /// </summary>
    public class UserGroups : FullAuditedEntity
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
        /// Aktiflik
        /// </summary>
        public bool IsActive { get; set; }

    }
}
