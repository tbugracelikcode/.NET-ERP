using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities;

namespace TsiErp.Entities.Entities.GeneralSystemIdentifications.UserPermission.Dtos
{
    public class ListUserPermissionsDto : IEntityDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Kullanıcı Adı
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Menu Adı
        /// </summary>
        public string MenuName { get; set; }
    }
}
