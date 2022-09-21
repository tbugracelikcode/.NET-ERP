using DevExpress.Blazor;
using DevExpress.Blazor.Base;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Inputs;
using LabelPosition = Syncfusion.Blazor.Buttons.LabelPosition;

namespace Tsi.Blazor.Component.Core.Components.DataEditors
{
    public abstract class BaseEditorParameters : DxComponentBase
    {
        [Parameter] public int ColumnIndex { get; set; }

        [Parameter] public int ColumnSpan { get; set; }

        [Parameter] public string LayoutItemCssClass { get; set; }

        [Parameter] public int RowIndex { get; set; }

        [Parameter] public int RowSpan { get; set; }

        [Parameter] public bool ReadOnly { get; set; }

        [Parameter] public string NullText { get; set; }

        [Parameter] public string SeparateCaption { get; set; } = ":";

        [Parameter] public virtual bool SeparateCaptionVisible { get; set; } = true;

        [Parameter] public virtual bool CaptionVisible { get; set; } = true;

        [Parameter] public string Caption { get; set; }

        [Parameter] public string CaptionCssClass { get; set; } = "caption";

        [Parameter] public bool Visible { get; set; } = true;
        [Parameter] public string InputCssClass { get; set; }


        [Parameter] public string Placeholder { get; set; }
        [Parameter] public bool EnableRtl { get; set; } = false;
        [Parameter] public FloatLabelType FloatLabelType { get; set; } = FloatLabelType.Auto;
        [Parameter] public InputType InputType { get; set; } = InputType.Text;
        [Parameter] public virtual string EditorCssClass { get; set; }
        [Parameter] public bool Enabled { get; set; } = true;
        [Parameter] public bool ShowClearButton { get; set; } = true;



        [Parameter] public LabelPosition LabelPosition { get; set; } = LabelPosition.Before;


        [Parameter] public bool Autofill { get; set; } = true;
        [Parameter] public bool AllowFiltering { get; set; } = true;

        [Parameter] public string Format { get; set; }
        [Parameter] public bool ShowSpinButton { get; set; }
        [Parameter] public bool StrictMode { get; set; }
    }
}
