using Tsi.Blazor.Component.Core.TsiComponents.Base.Interfaces;
using Tsi.Blazor.Component.Core.TsiComponents.Enums;

namespace Tsi.Blazor.Component.Core.TsiComponents.Base.Providers
{
    public abstract class ClassProvider : IClassProvider
    {
        #region TextEdit

        public abstract string TextEdit(bool plaintext);

        public abstract string TextEditSize(Size size);

        public abstract string TextEditColor(Color color);


        #endregion



        #region Text
        public abstract string TextAlignment(TextAlignment textAlignment);

        public abstract string TextWeight(TextWeight textWeight);

        public abstract string TextOverflow(TextOverflow textOverflow);

        public abstract string TextItalic();

        #endregion

        #region Float

        public virtual string Float(Float @float) => $"float-{ToFloat(@float)}";

        public virtual string Clearfix() => "clearfix";

        #endregion

        #region Visibility

        public abstract string Visibility(Visibility visibility);

        #endregion

        #region Enums


        public virtual string ToSize(Size size)
        {
            return size switch
            {
                Size.ExtraSmall => "xs",
                Size.Small => "sm",
                Size.Medium => "md",
                Size.Large => "lg",
                Size.ExtraLarge => "xl",
                _ => null,
            };
        }

        public virtual string ToFloat(Float @float)
        {
            return @float switch
            {
                Tsi.Blazor.Component.Core.TsiComponents.Enums.Float.Start => "left",
                Tsi.Blazor.Component.Core.TsiComponents.Enums.Float.End => "right",
                _ => null,
            };
        }

        public virtual string ToColor(Color color)
        {
            return color?.Name;
        }

        public virtual string ToTextAlignment(TextAlignment textAlignment)
        {
            return textAlignment switch
            {
                Tsi.Blazor.Component.Core.TsiComponents.Enums.TextAlignment.Left => "left",
                Tsi.Blazor.Component.Core.TsiComponents.Enums.TextAlignment.Center => "center",
                Tsi.Blazor.Component.Core.TsiComponents.Enums.TextAlignment.Right => "right",
                Tsi.Blazor.Component.Core.TsiComponents.Enums.TextAlignment.Justified => "justify",
                _ => null,
            };
        }

        public virtual string ToTextWeight(TextWeight textWeight)
        {
            return textWeight switch
            {
                Tsi.Blazor.Component.Core.TsiComponents.Enums.TextWeight.Normal => "normal",
                Tsi.Blazor.Component.Core.TsiComponents.Enums.TextWeight.Bold => "bold",
                Tsi.Blazor.Component.Core.TsiComponents.Enums.TextWeight.Light => "light",
                _ => null,
            };
        }

        public virtual string ToTextOverflow(TextOverflow textOverflow)
        {
            return textOverflow switch
            {
                Tsi.Blazor.Component.Core.TsiComponents.Enums.TextOverflow.Wrap => "wrap",
                Tsi.Blazor.Component.Core.TsiComponents.Enums.TextOverflow.NoWrap => "nowrap",
                Tsi.Blazor.Component.Core.TsiComponents.Enums.TextOverflow.Truncate => "truncate",
                _ => null,
            };
        }

        #endregion
    }
}
