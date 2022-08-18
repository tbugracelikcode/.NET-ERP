using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Tsi.IoC.Tsi.DependencyResolvers
{
    public static class DependencyInjectionExtensions
    {
        public static void RegisterDependencies(this IServiceCollection services, Assembly assembly)
        {
            var types = assembly
                .ExportedTypes
                .Where(x => x.GetCustomAttributes(typeof(ServiceRegistrationAttribute), true).Length > 0)
                .ToList();

            for (var i = 0; i < types.Count; i++)
            {
                var currentType = types[i];
                var attributes = (ServiceRegistrationAttribute[])currentType.GetCustomAttributes(typeof(ServiceRegistrationAttribute),
                    true);
                var attribute = attributes[0];
                var implementedInterface = attribute.ImplementedInterface;
                switch (attribute.Scope)
                {
                    case DependencyInjectionType.Scoped:
                        services.AddScoped(implementedInterface, currentType);
                        break;
                    case DependencyInjectionType.Transient:
                        services.AddTransient(implementedInterface, currentType);
                        break;
                    case DependencyInjectionType.Singleton:
                        services.AddSingleton(implementedInterface, currentType);
                        break;
                }
            }
        }
    }
}
