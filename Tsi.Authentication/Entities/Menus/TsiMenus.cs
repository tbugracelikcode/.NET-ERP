using System;
using System.Collections.Generic;
using System.Text;
using Tsi.Authentication.Entities.RolePermissions;
using Tsi.Core.Entities;
using Tsi.Core.Entities.Auditing;

namespace Tsi.Authentication.Entities.Menus
{
    public class TsiMenus : IEntity
    {
        public Guid Id { get; set; }

        public string MenuName { get; set; }

        public Guid ParentMenutId { get; set; }

        public ICollection<TsiRolePermissions> TsiRolePermissions { get; set; }
    }
}
