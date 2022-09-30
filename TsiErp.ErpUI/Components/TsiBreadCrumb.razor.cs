using Microsoft.AspNetCore.Components;

namespace TsiErp.ErpUI.Components
{
    public partial class TsiBreadCrumb
    {
        [Parameter, EditorRequired] public string PreviousMenus { get; set; }

        [Parameter, EditorRequired] public string CurrentMenu { get; set; }
    }
}
