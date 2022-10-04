using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Blazor.Component.Core.TsiComponents.Enums;
using Tsi.Blazor.Component.Core.TsiComponents.Extensions;

namespace Tsi.Blazor.Component.Core.TsiComponents.Base
{
    public abstract class BaseTextInput<TValue> : BaseInputComponent<TValue>
    {

        private Color color = Color.Default;

        private ValueDebouncer inputValueDebouncer;


        protected override void OnInitialized()
        {
            if (IsDebounce)
            {
                inputValueDebouncer = new(DebounceIntervalValue);
                inputValueDebouncer.Debounce += OnInputValueDebounce;
            }

            base.OnInitialized();
        }

        
        protected virtual Task OnChangeHandler(ChangeEventArgs eventArgs)
        {
            if (!IsImmediate)
            {
                return CurrentValueHandler(eventArgs?.Value?.ToString());
            }

            return Task.CompletedTask;
        }

        
        protected virtual Task OnInputHandler(ChangeEventArgs eventArgs)
        {
            if (IsImmediate)
            {
                var value = eventArgs?.Value?.ToString();
                if (IsDebounce)
                {
                    inputValueDebouncer?.Update(value);
                }
                else
                {
                    return CurrentValueHandler(value);
                }
            }

            return Task.CompletedTask;
        }


        protected override Task OnKeyPressHandler(KeyboardEventArgs eventArgs)
        {
            if (IsImmediate
                && IsDebounce
                && (eventArgs?.Key?.Equals("Enter", StringComparison.OrdinalIgnoreCase) ?? false))
            {
                inputValueDebouncer?.Flush();
            }

            return base.OnKeyPressHandler(eventArgs);
        }

        protected override Task OnBlurHandler(FocusEventArgs eventArgs)
        {
            if (IsImmediate
                && IsDebounce)
            {
                inputValueDebouncer?.Flush();
            }

            return base.OnBlurHandler(eventArgs);
        }

        private void OnInputValueDebounce(object sender, string value)
        {
            InvokeAsync(() => CurrentValueHandler(value));
        }

        protected bool IsImmediate
            => Immediate.GetValueOrDefault(true);

        protected bool IsDebounce
            => Debounce.GetValueOrDefault(false);

        protected int DebounceIntervalValue
            => DebounceInterval.GetValueOrDefault(300);

        protected string BindValueEventName
            => IsImmediate ? "oninput" : "onchange";

        protected ValueDebouncer InputValueDebouncer => inputValueDebouncer;

        [Parameter] public string Placeholder { get; set; }

        [Parameter] public bool Plaintext { get; set; }

        [Parameter]
        public Color Color
        {
            get => color;
            set
            {
                color = value;
            }
        }

        [Parameter] public string Pattern { get; set; }

        [Parameter] public bool? Immediate { get; set; }

        [Parameter] public bool? Debounce { get; set; }

        [Parameter] public int? DebounceInterval { get; set; }

    }
}
