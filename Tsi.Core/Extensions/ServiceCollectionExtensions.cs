using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Tsi.Core.Modularity;
using Tsi.Core.Utilities.IoC;

namespace Tsi.Core.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDependencyResolvers(this IServiceCollection services,
            TsiModule[] modules)
        {
            foreach (var module in modules)
            {
                module.ConfigureServices(services);
            }

            return ServiceTool.Create(services);
        }
    }
}
