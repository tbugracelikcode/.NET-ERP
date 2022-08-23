using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Tsi.Core.Extensions;

namespace Tsi.Core.Modularity.Extension
{
    public static class DependenTypeProvideExtensions
    {
        public static void RegisterRelatedModuleAssemblies(this IServiceCollection services, MemberInfo element)
        {
            List<Type> dependencies = new List<Type>();

            var dependencyDescriptors = element.GetCustomAttributes().OfType<IDependedTypesProvider>();

            foreach (var descriptor in dependencyDescriptors)
            {
                foreach (var dependedModuleType in descriptor.GetDependedTypes())
                {
                    dependencies.Add(dependedModuleType);
                }
            }

            foreach (var item in dependencies)
            {
                var instance = (TsiModule)Activator.CreateInstance(item);

                var descriptor = new ServiceDescriptor(typeof(TsiModule), instance);
                
                if (!services.Contains(descriptor))
                {
                    services.AddDependencyResolvers(new TsiModule[]
                    {
                        instance
                    });
                }
            }
        }
    }
}
