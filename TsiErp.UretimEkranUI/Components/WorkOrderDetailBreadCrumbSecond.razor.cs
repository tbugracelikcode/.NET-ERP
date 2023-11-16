using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TsiErp.UretimEkranUI.Components
{
    public partial class WorkOrderDetailBreadCrumbSecond
    {

        [Parameter, EditorRequired] public string StationCode { get; set; }

        [Parameter, EditorRequired] public string OperationName { get; set; }

        [Parameter, EditorRequired] public string EmployeeName { get; set; }
    }
}
