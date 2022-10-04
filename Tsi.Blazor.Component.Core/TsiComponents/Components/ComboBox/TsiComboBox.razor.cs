using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Tsi.Blazor.Component.Core.TsiComponents.Base;
using Tsi.Blazor.Component.Core.TsiComponents.Base.Providers;
using Tsi.Blazor.Component.Core.TsiComponents.Extensions;
using Tsi.Blazor.Component.Core.TsiComponents.Utilities;

namespace Tsi.Blazor.Component.Core.TsiComponents.Components.ComboBox
{
    public partial class TsiComboBox<TValue> : BaseInputComponent<IReadOnlyList<TValue>>
    {

        private bool multiple;

        private bool loading;

        private readonly List<ISelectItem<TValue>> selectItems = new();

        private const string MULTIPLE_DELIMITER = ",";




        public override async Task SetParametersAsync(ParameterView parameters)
        {
            await base.SetParametersAsync(parameters);
        }


        protected virtual Task OnChangeHandler(ChangeEventArgs eventArgs)
        {
            var value = Multiple && eventArgs?.Value is string[] multiValues
                ? string.Join(MULTIPLE_DELIMITER, multiValues)
                : eventArgs?.Value?.ToString();

            return CurrentValueHandler(value);
        }


        protected override Task OnInternalValueChanged(IReadOnlyList<TValue> value)
        {
            if (Multiple)
                return SelectedValuesChanged.InvokeAsync(value);
            else
                return SelectedValueChanged.InvokeAsync(value == null ? default : value.FirstOrDefault());
        }



        protected override Task<ParseValue<IReadOnlyList<TValue>>> ParseValueFromStringAsync(string value)
        {
            if (string.IsNullOrEmpty(value))
                return Task.FromResult(ParseValue<IReadOnlyList<TValue>>.Empty);

            if (Multiple)
            {
                var multipleValues = value.Split(MULTIPLE_DELIMITER).Select(value =>
                {
                    if (Converters.TryChangeType<TValue>(value, out var newValue))
                        return newValue;

                    return default;
                }).ToArray();

                return Task.FromResult(new ParseValue<IReadOnlyList<TValue>>(true, multipleValues, null));
            }
            else
            {
                if (Converters.TryChangeType<TValue>(value, out var result))
                {
                    return Task.FromResult(new ParseValue<IReadOnlyList<TValue>>(true, new TValue[] { result }, null));
                }
                else
                {
                    return Task.FromResult(ParseValue<IReadOnlyList<TValue>>.Empty);
                }
            }
        }


        protected override string FormatValueAsString(IReadOnlyList<TValue> value)
        {
            if (value == null || value.Count == 0)
                return string.Empty;

            if (Multiple)
            {
                // Make use of .NET BindConverter that will convert our array into valid JSON string.
                return BindConverter.FormatValue(CurrentValue?.ToArray() ?? new TValue[] { })?.ToString();
            }
            else
            {
                if (value[0] == null)
                    return string.Empty;

                return value[0].ToString();
            }
        }

        public void ClearButtonClicked()
        {
            CurrentValue = null;
        }


        public bool ContainsValue(TValue value)
        {
            var currentValue = CurrentValue;

            if (currentValue != null)
            {
                var result = currentValue.Any(x => x.IsEqual(value));

                return result;
            }

            return false;
        }

        internal void NotifySelectItemInitialized(ISelectItem<TValue> selectItem)
        {
            if (selectItem == null)
                return;

            if (!selectItems.Contains(selectItem))
                selectItems.Add(selectItem);
        }

        internal void NotifySelectItemRemoved(ISelectItem<TValue> selectItem)
        {
            if (selectItem == null)
                return;

            if (selectItems.Contains(selectItem))
                selectItems.Remove(selectItem);
        }


        protected override IReadOnlyList<TValue> InternalValue
        {
            get => Multiple ? SelectedValues : new TValue[] { SelectedValue };
            set
            {
                if (Multiple)
                {
                    SelectedValues = value;
                }
                else
                {
                    SelectedValue = value == null ? default : value.FirstOrDefault();
                }
            }
        }


        protected IEnumerable<ISelectItem<TValue>> SelectItems => selectItems;


        [Parameter]
        public bool Multiple
        {
            get => multiple;
            set
            {
                multiple = value;
            }
        }

        [Parameter] public bool ShowClearButton { get; set; }

        [Parameter] public TValue SelectedValue { get; set; }


        [Parameter] public IReadOnlyList<TValue> SelectedValues { get; set; }


        [Parameter] public EventCallback<TValue> SelectedValueChanged { get; set; }


        [Parameter] public EventCallback<IReadOnlyList<TValue>> SelectedValuesChanged { get; set; }


        [Parameter] public Expression<Func<TValue>> SelectedValueExpression { get; set; }


        [Parameter] public Expression<Func<IReadOnlyList<TValue>>> SelectedValuesExpression { get; set; }


        [Parameter] public int? MaxVisibleItems { get; set; }

        [Parameter] public string ClassNames { get; set; }

        [Parameter] public string StyleNames { get; set; }


        [Parameter]
        public bool Loading
        {
            get => loading;
            set
            {
                loading = value;
                Disabled = value;
            }
        }

    }
}
