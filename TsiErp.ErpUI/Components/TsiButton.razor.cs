using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace TsiErp.ErpUI.Components
{
    public partial class TsiButton
    {
        [Parameter]
        public bool Enabled { get; set; } = true;
        [Parameter]
        public EventCallback<MouseEventArgs> ButtonClick { get; set; }

        [Parameter] 
        public Type ButtonType { get; set; }

        public enum Type
        {
            Save,
            Delete,
            Cancel,
            LogOut,
            ShowColumns,
            Submit
        }

        private async Task ClickAsync()
        {
            if (Enabled)
            {
                await ButtonClick.InvokeAsync();
            }
        }
    }
}
