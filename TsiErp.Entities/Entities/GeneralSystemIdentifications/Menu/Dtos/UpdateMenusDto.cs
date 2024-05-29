using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;
using Tsi.Core.Utilities.SqlDataTypeMappingUtilities;

namespace TsiErp.Entities.Entities.GeneralSystemIdentifications.Menu.Dtos
{
    public class UpdateMenusDto
    {
        public Guid Id { get; set; }

        public string MenuName { get; set; }

        public Guid ParentMenuId { get; set; }
        public int ContextOrderNo { get; set; }
        public string MenuURL { get; set; }
    }
}
