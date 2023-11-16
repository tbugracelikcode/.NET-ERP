using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TsiErp.UretimEkranUI.Components
{
    public partial class WorkOrderDetailBreadCrumbFirst
    {
        [Parameter, EditorRequired] public string WorkOrderNo { get; set; }

        [Parameter, EditorRequired] public decimal PlannedAmount { get; set; }

        [Parameter, EditorRequired] public string ProductName { get; set; }
    }
}
