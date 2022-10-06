using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Blazor.Component.Core.TsiComponents.Enums;

namespace Tsi.Blazor.Component.Core.TsiComponents.Extensions
{
    public static class EnumExtensions
    {
        public static string ToTextRoleString(this TextRole textRole)
        {
            return textRole switch
            {
                TextRole.Email => "email",
                TextRole.Password => "password",
                TextRole.Url => "url",
                TextRole.Search => "search",
                TextRole.Telephone => "tel",
                _ => "text",
            };
        }

        public static string ToTextInputMode(this TextInputMode textInputMode)
        {
            return textInputMode switch
            {
                TextInputMode.Text => "text",
                TextInputMode.Tel => "tel",
                TextInputMode.Url => "url",
                TextInputMode.Email => "email",
                TextInputMode.Numeric => "numeric",
                TextInputMode.Decimal => "decimal",
                TextInputMode.Search => "search",
                _ => null,
            };
        }
    }
}
