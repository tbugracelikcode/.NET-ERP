using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities;
using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.GeneralSystemIdentifications.Menu.Dtos
{
    public class SelectMenusDto : IEntityDto
    {
        public Guid Id { get; set; }

        public string MenuName { get; set; }

        public Guid ParentMenuId { get; set; }
        public int ContextOrderNo { get; set; }
    }
}
