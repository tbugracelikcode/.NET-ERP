using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.GeneralSystemIdentifications.UserGroup.Dtos
{
    public class UpdateUserGroupsDto : FullAuditedEntityDto
    {
        /// <summary>
        /// Kullanıcı Grubu Kodu
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Kullanıcı Grubu Adı
        /// </summary>
        public string Name { get; set; }
    }
}
