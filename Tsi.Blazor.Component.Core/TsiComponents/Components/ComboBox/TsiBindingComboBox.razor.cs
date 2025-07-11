﻿using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Tsi.Blazor.Component.Core.TsiComponents.Enums;

namespace Tsi.Blazor.Component.Core.TsiComponents.Components.ComboBox
{
    public partial class TsiBindingComboBox<TItem, TValue> : ComponentBase
    {
        #region Members

        /// <summary>
        /// Reference to the <see cref="Select{TValue}"/> component.
        /// </summary>
        private TsiComboBox<TValue> selectRef;

        #endregion

        #region Methods

        protected Task HandleSelectedValueChanged(TValue value)
        {
            SelectedValue = value;

            return SelectedValueChanged.InvokeAsync(value);
        }

        protected Task HandleSelectedValuesChanged(IReadOnlyList<TValue> value)
        {
            SelectedValues = value;

            return SelectedValuesChanged.InvokeAsync(value);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the select element id.
        /// </summary>
        [Parameter] public string ElementId { get; set; }


        /// <summary>
        /// Specifies that multiple items can be selected.
        /// </summary>
        [Parameter] public bool Multiple { get; set; }

        /// <summary>
        /// Gets or sets the select data-source.
        /// </summary>
        [Parameter] public IEnumerable<TItem> Data { get; set; }

        /// <summary>
        /// Method used to get the display field from the supplied data source.
        /// </summary>
        [Parameter] public Func<TItem, string> TextField { get; set; }

        /// <summary>
        /// Method used to get the value field from the supplied data source.
        /// </summary>
        [Parameter] public Func<TItem, TValue> ValueField { get; set; }

        /// <summary>
        /// Method used to determine if an item should be disabled.
        /// </summary>
        [Parameter] public Func<TItem, bool> ItemDisabled { get; set; }

        /// <summary>
        /// Currently selected item value.
        /// </summary>
        [Parameter] public TValue SelectedValue { get; set; }

        /// <summary>
        /// Gets or sets the multiple selected item values.
        /// </summary>
        [Parameter] public IReadOnlyList<TValue> SelectedValues { get; set; }

        /// <summary>
        /// Occurs after the selected value has changed.
        /// </summary>
        [Parameter] public EventCallback<TValue> SelectedValueChanged { get; set; }

        /// <summary>
        /// Occurs when the selected items value has changed (only when <see cref="Multiple"/>==true).
        /// </summary>
        [Parameter] public EventCallback<IReadOnlyList<TValue>> SelectedValuesChanged { get; set; }

        /// <summary>
        /// Gets or sets an expression that identifies the selected value.
        /// </summary>
        [Parameter] public Expression<Func<TValue>> SelectedValueExpression { get; set; }

        /// <summary>
        /// Gets or sets an expression that identifies the selected value.
        /// </summary>
        [Parameter] public Expression<Func<IReadOnlyList<TValue>>> SelectedValuesExpression { get; set; }

        /// <summary>
        /// Display text of the default select item.
        /// </summary>
        [Parameter] public string DefaultItemText { get; set; }

        /// <summary>
        /// Value of the default select item.
        /// </summary>
        [Parameter] public TValue DefaultItemValue { get; set; }

        /// <summary>
        /// If true, disables the default item.
        /// </summary>
        [Parameter] public bool DefaultItemDisabled { get; set; } = false;

        /// <summary>
        /// If true, disables the default item.
        /// </summary>
        [Parameter] public bool DefaultItemHidden { get; set; } = false;

        /// <summary>
        /// Custom css class-names.
        /// </summary>
        [Parameter] public string Class { get; set; }

        /// <summary>
        /// Custom styles.
        /// </summary>
        [Parameter] public string Style { get; set; }

        /// <summary>
        /// Size of a select field.
        /// </summary>
        [Parameter] public Size? Size { get; set; }

        /// <summary>
        /// Specifies how many options should be shown at once.
        /// </summary>
        [Parameter] public int? MaxVisibleItems { get; set; }

        /// <summary>
        /// If defined, indicates that its element can be focused and can participates in sequential keyboard navigation.
        /// </summary>
        [Parameter] public int? TabIndex { get; set; }

        /// <summary>
        /// Add the disabled boolean attribute on an select to prevent user interactions and make it appear lighter.
        /// </summary>
        [Parameter] public bool Disabled { get; set; }

        /// <summary>
        /// Captures all the custom attribute that are not part of Blazorise component.
        /// </summary>
        [Parameter(CaptureUnmatchedValues = true)]
        public Dictionary<string, object> Attributes { get; set; }

        /// <summary>
        /// Placeholder for validation messages.
        /// </summary>
        [Parameter] public RenderFragment Feedback { get; set; }

        /// <summary>
        /// Specifies the content to be rendered inside this <see cref="SelectList{TItem, TValue}"/>.
        /// </summary>
        [Parameter] public RenderFragment ChildContent { get; set; }

        #endregion
    }
}
