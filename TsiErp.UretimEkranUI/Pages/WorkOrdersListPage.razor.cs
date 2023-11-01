using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TsiErp.UretimEkranUI.Pages
{
    public partial class WorkOrdersListPage
    {

        protected override void OnInitialized()
        {
            BaseCrudService = WorkOrdersAppService;
        }
    }
}
