using DevExpress.XtraPrinting;
using Syncfusion.Blazor.Inputs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Blazor.Component.Core.TsiComponents.Base.Providers;
using Tsi.Blazor.Component.Core.TsiComponents.Enums;

namespace Tsi.Blazor.Component.Core.TsiComponents
{
    public class Bootstrap5ClassProvider : ClassProvider
    {
        #region TextEdit

        public override string TextEdit(bool plaintext) => plaintext ? "form-control-plaintext" : "form-control";

        public override string TextEditSize(Size size) => $"form-control-{ToSize(size)}";

        public override string TextEditColor(Color color) => $"text-{ToColor(color)}";

        #endregion



        #region Text


        public override string TextAlignment(Tsi.Blazor.Component.Core.TsiComponents.Enums.TextAlignment textAlignment) => $"text-{ToTextAlignment(textAlignment)}";


        public override string TextWeight(TextWeight textWeight) => $"fw-{ToTextWeight(textWeight)}";

        public override string TextOverflow(TextOverflow textOverflow) => $"text-{ToTextOverflow(textOverflow)}";

        public override string TextItalic() => "fst-italic";

        #endregion

        #region Visibility

        public override string Visibility(Visibility visibility)
        {
            return visibility switch
            {
                Tsi.Blazor.Component.Core.TsiComponents.Enums.Visibility.Visible => "visible",
                Tsi.Blazor.Component.Core.TsiComponents.Enums.Visibility.Invisible => "invisible",
                _ => null,
            };
        }

        #endregion

        #region Enums

        public override string ToFloat(Float @float)
        {
            return @float switch
            {
                Tsi.Blazor.Component.Core.TsiComponents.Enums.Float.Start => "start",
                Tsi.Blazor.Component.Core.TsiComponents.Enums.Float.End => "end",
                _ => null,
            };
        }

        public override string ToTextAlignment(Tsi.Blazor.Component.Core.TsiComponents.Enums.TextAlignment textAlignment)
        {
            return textAlignment switch
            {
                Tsi.Blazor.Component.Core.TsiComponents.Enums.TextAlignment.Left => "start",
                Tsi.Blazor.Component.Core.TsiComponents.Enums.TextAlignment.Center => "center",
                Tsi.Blazor.Component.Core.TsiComponents.Enums.TextAlignment.Right => "end",
                Tsi.Blazor.Component.Core.TsiComponents.Enums.TextAlignment.Justified => "justify",
                _ => null,
            };
        }

        #endregion
    }
}
