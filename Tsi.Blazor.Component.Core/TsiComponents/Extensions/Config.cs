using DevExpress.Utils.Filtering.Internal;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Tsi.Blazor.Component.Core.TsiComponents.Base.Interfaces;

namespace Tsi.Blazor.Component.Core.TsiComponents.Extensions
{
    public static class Config
    {
        public static IServiceCollection AddBootstrap5Providers(this IServiceCollection serviceCollection, Action<IClassProvider> configureClassProvider = null)
        {
            var classProvider = new Bootstrap5ClassProvider();

            configureClassProvider?.Invoke(classProvider);

            serviceCollection.AddSingleton<IClassProvider>(classProvider);
            //serviceCollection.AddSingleton<IStyleProvider, Bootstrap5StyleProvider>();
            //serviceCollection.AddSingleton<IBehaviourProvider, Bootstrap5BehaviourProvider>();
            //serviceCollection.AddScoped<IThemeGenerator, Bootstrap5ThemeGenerator>();
            //serviceCollection.AddBootstrap5Components();

            //serviceCollection.AddScoped<IJSModalModule, Modules.BootstrapJSModalModule>();
            //serviceCollection.AddScoped<IJSTooltipModule, Modules.BootstrapJSTooltipModule>();

            return serviceCollection;
        }
    }
}
