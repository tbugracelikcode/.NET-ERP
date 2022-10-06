using DevExpress.Blazor;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Tsi.Blazor.Component.Core.TsiComponents.Enums;

namespace Tsi.Blazor.Component.Core.TsiComponents.Base
{
    public abstract class BaseComponent : ComponentBase
    {
        private string customStyle;

        private string customClass;

        private Float @float = Float.Default;

        private Visibility visibility = Visibility.Default;

        private TextAlignment textAlignment = TextAlignment.Default;

        private TextWeight textWeight = TextWeight.Default;

        private TextOverflow textOverflow = TextOverflow.Default;

        public ElementReference ElementRef { get; set; }

        [Parameter] public string ElementId { get; set; }

        [Parameter]
        public string Class
        {
            get => customClass;
            set
            {
                customClass = value;
            }
        }

        [Parameter]
        public string Style
        {
            get => customStyle;
            set
            {
                customStyle = value;
            }
        }

        [Parameter]
        public Float Float
        {
            get => @float;
            set
            {
                @float = value;
            }
        }

        [Parameter]
        public Visibility Visibility
        {
            get => visibility;
            set
            {
                visibility = value;
            }
        }

        [Parameter]
        public TextAlignment TextAlignment
        {
            get => textAlignment;
            set
            {
                textAlignment = value;
            }
        }

        [Parameter]
        public TextWeight TextWeight
        {
            get => textWeight;
            set
            {
                textWeight = value;
            }
        }

        [Parameter]
        public TextOverflow TextOverflow
        {
            get => textOverflow;
            set
            {
                textOverflow = value;
            }
        }




        [Parameter(CaptureUnmatchedValues = true)]
        public Dictionary<string, object> Attributes { get; set; }

        public override Task SetParametersAsync(ParameterView parameters)
        {
            object heightAttribute = null;

            if (parameters.TryGetValue("width", out object widthAttribute)
                || parameters.TryGetValue("height", out heightAttribute))
            {
                var parametersDictionary = (Dictionary<string, object>)parameters.ToDictionary();

                Attributes ??= new();

                if (widthAttribute != null && parametersDictionary.Remove("width"))
                {
                    Attributes.Add("width", widthAttribute);
                }

                if (heightAttribute != null && parametersDictionary.Remove("height"))
                {
                    Attributes.Add("height", heightAttribute);
                }

                return base.SetParametersAsync(ParameterView.FromDictionary(parametersDictionary));
            }

            return base.SetParametersAsync(parameters);
        }


        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await OnFirstAfterRenderAsync();
            }

            await base.OnAfterRenderAsync(firstRender);
        }

        protected virtual Task OnFirstAfterRenderAsync()
            => Task.CompletedTask;



    }
}
