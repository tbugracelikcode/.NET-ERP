using System;
using System.Collections.Generic;
using System.Text;
using Tsi.Core.Entities.Auditing;

namespace Tsi.Authentication.Dtos.Menus
{
    public class ListMenusDto : FullAuditedEntityDto
    {
        public string MenuName { get; set; }

        public Guid ParentMenuId { get; set; }
    }
}
