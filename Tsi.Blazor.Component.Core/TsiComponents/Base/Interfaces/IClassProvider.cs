
using Tsi.Blazor.Component.Core.TsiComponents.Enums;

namespace Tsi.Blazor.Component.Core.TsiComponents.Base.Interfaces
{
    public interface IClassProvider
    {

        #region TextEdit

        string TextEdit(bool plaintext);

        string TextEditSize(Size size);

        string TextEditColor(Color color);

        #endregion


        #region Text

        string TextAlignment(TextAlignment textAlignment);

        string TextWeight(TextWeight textWeight);

        string TextOverflow(TextOverflow textOverflow);

        string TextItalic();

        #endregion

        #region Visibility

        string Visibility(Visibility visibility);

        #endregion

        #region Float

        string Float(Float @float);

        string Clearfix();

        #endregion

        #region Enums

        string ToFloat(Float @float);

        string ToSize(Size size);

        string ToColor(Color color);

        string ToTextAlignment(TextAlignment textAlignment);

        string ToTextWeight(TextWeight textWeight);

        string ToTextOverflow(TextOverflow textOverflow);

        #endregion

    }
}
