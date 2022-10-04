using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Blazor.Component.Core.TsiComponents.Base;

namespace Tsi.Blazor.Component.Core.TsiComponents.Components.ComboBox
{
    public partial class TsiComboBoxGroup : BaseComponent
    {
        #region Properties

        /// <summary>
        /// Gets or sets the group label.
        /// </summary>
        [Parameter] public string Label { get; set; }

        /// <summary>
        /// Specifies the content to be rendered inside this <see cref="SelectGroup"/>.
        /// </summary>
        [Parameter] public RenderFragment ChildContent { get; set; }

        [Parameter] public string ClassNames { get; set; }

        [Parameter] public string StyleNames { get; set; }

        #endregion
    }
}
