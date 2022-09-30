using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TsiErp.ErpUI.Components
{
    public partial class TsiTextBox
    {
        [Parameter]
        public int TSISelectedID { get; set; }

        [Parameter]
        public string Placeholder { get; set; }

        private string _tsiText;

        [Parameter]
        public EventCallback<string> TSITextChanged { get; set; }

        [Parameter]
        public string TSIText
        {
            get => _tsiText;
            set
            {
                if (_tsiText == value) return;
                _tsiText = value;
                TSITextChanged.InvokeAsync(value);

            }
        }

        [Parameter]
        public bool Disable { get; set; }
    }
}
