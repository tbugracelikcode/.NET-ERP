using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Tsi.IoC.IoC.Autofac.ServiceTools;

namespace Tsi.IoC.IoC.Autofac.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDependencyResolvers(this IServiceCollection services, ITsiCoreService[] modules)
        {
            foreach (var module in modules)
            {
                module.RegisterService(services);
            }

            return ServiceTool.Create(services);
        }
    }
}
