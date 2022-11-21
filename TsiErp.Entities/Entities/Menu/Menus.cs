using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.Menu
{
    public class Menus : FullAuditedEntity
    {
        public string MenuName { get; set; }

        public Guid ParentMenuId { get; set; }
    }
}
