using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Blazor.Component.Core.TsiComponents.Base;

namespace Tsi.Blazor.Component.Core.TsiComponents.Components.ComboBox
{
    public interface ISelectItem<TValue>
    {
        TValue Value { get; set; }

        RenderFragment ChildContent { get; set; }
    }

    public partial class TsiComboBoxItem<TValue> : BaseComponent, ISelectItem<TValue>
    {
        #region Methods

        /// <inheritdoc/>
        protected override Task OnInitializedAsync()
        {
            ParentSelect?.NotifySelectItemInitialized(this);

            return base.OnInitializedAsync();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the flag that indicates if item is selected.
        /// </summary>
        protected bool Selected => ParentSelect?.ContainsValue(Value) == true;

        /// <summary>
        /// Convert the value to string because option tags are working with string internally. Otherwise some datatypes like booleans will not work as expected.
        /// </summary>
        protected string StringValue => Value?.ToString();

        /// <summary>
        /// Gets or sets the item value.
        /// </summary>
        [Parameter] public TValue Value { get; set; }

        /// <summary>
        /// Disable the item from mouse click.
        /// </summary>
        [Parameter] public bool Disabled { get; set; }

        /// <summary>
        /// Hide the item from the list so it can be used as a placeholder.
        /// </summary>
        [Parameter] public bool Hidden { get; set; }

        /// <summary>
        /// Specifies the select component in which this select item is placed.
        /// </summary>
        [CascadingParameter] protected virtual TsiComboBox<TValue> ParentSelect { get; set; }

        /// <summary>
        /// Specifies the content to be rendered inside this <see cref="SelectItem{TValue}"/>.
        /// </summary>
        [Parameter] public RenderFragment ChildContent { get; set; }



        [Parameter] public string ClassNames { get; set; }

        [Parameter] public string StyleNames { get; set; }

        #endregion
    }
}
