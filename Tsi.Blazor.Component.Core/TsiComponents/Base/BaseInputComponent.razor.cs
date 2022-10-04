using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Syncfusion.Blazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Blazor.Component.Core.TsiComponents.Enums;
using Tsi.Blazor.Component.Core.TsiComponents.Extensions;

namespace Tsi.Blazor.Component.Core.TsiComponents.Base
{
    public abstract class BaseInputComponent<TValue> : BaseComponent
    {
        private Size? size;

        private bool readOnly;

        private bool disabled;

        [Parameter] public EventCallback<KeyboardEventArgs> KeyDown { get; set; }

        [Parameter] public EventCallback<KeyboardEventArgs> KeyPress { get; set; }

        [Parameter] public EventCallback<KeyboardEventArgs> KeyUp { get; set; }

        [Parameter] public EventCallback<FocusEventArgs> Blur { get; set; }

        [Parameter] public EventCallback<FocusEventArgs> OnFocus { get; set; }

        [Parameter] public EventCallback<FocusEventArgs> FocusIn { get; set; }

        [Parameter] public EventCallback<FocusEventArgs> FocusOut { get; set; }

        protected virtual Task OnKeyDownHandler(KeyboardEventArgs eventArgs)
        {
            return KeyDown.InvokeAsync(eventArgs);
        }

        protected virtual Task OnKeyPressHandler(KeyboardEventArgs eventArgs)
        {
            return KeyPress.InvokeAsync(eventArgs);
        }

        protected virtual Task OnKeyUpHandler(KeyboardEventArgs eventArgs)
        {
            return KeyUp.InvokeAsync(eventArgs);
        }

        protected virtual Task OnBlurHandler(FocusEventArgs eventArgs)
        {
            return Blur.InvokeAsync(eventArgs);
        }

        protected virtual Task OnFocusHandler(FocusEventArgs eventArgs)
        {
            return OnFocus.InvokeAsync(eventArgs);
        }

        protected virtual Task OnFocusInHandler(FocusEventArgs eventArgs)
        {
            return FocusIn.InvokeAsync(eventArgs);
        }

        protected virtual Task OnFocusOutHandler(FocusEventArgs eventArgs)
        {
            return FocusOut.InvokeAsync(eventArgs);
        }


        protected abstract Task OnInternalValueChanged(TValue value);

        protected virtual bool IsSameAsInternalValue(TValue value) => value.IsEqual(InternalValue);

        protected virtual string FormatValueAsString(TValue value) => value?.ToString();

        protected virtual TValue DefaultValue => default;

      
        protected abstract TValue InternalValue { get; set; }

        protected TValue CurrentValue
        {
            get => InternalValue;
            set
            {
                if (!IsSameAsInternalValue(value))
                {
                    InternalValue = value;
                    InvokeAsync(() => OnInternalValueChanged(value));
                }
            }
        }

        protected abstract Task<ParseValue<TValue>> ParseValueFromStringAsync(string value);

        protected async Task CurrentValueHandler(string value)
        {
            var empty = false;

            if (string.IsNullOrEmpty(value))
            {
                empty = true;
                CurrentValue = DefaultValue;
            }

            if (!empty)
            {
                var result = await ParseValueFromStringAsync(value);

                if (result.Success)
                {
                    CurrentValue = result.ParsedValue;
                }
            }
        }

        protected string CurrentValueAsString
        {
            get => FormatValueAsString(CurrentValue);
            set
            {
                InvokeAsync(() => CurrentValueHandler(value));
            }
        }

        [Parameter]
        public Size? Size
        {
            get => size;
            set
            {
                size = value;
            }
        }

        [Parameter]
        public bool ReadOnly
        {
            get => readOnly;
            set
            {
                readOnly = value;
            }
        }

        [Parameter]
        public bool Disabled
        {
            get => disabled;
            set
            {
                disabled = value;
            }
        }

        [Parameter] public bool Autofocus { get; set; }

        [Parameter] public RenderFragment Feedback { get; set; }

        [Parameter] public RenderFragment ChildContent { get; set; }

       
        [Parameter] public int? TabIndex { get; set; }

    }
}
