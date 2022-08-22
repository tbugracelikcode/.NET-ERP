using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.CrossCuttingConcerns.Caching;
using Tsi.Core.Extensions;
using Tsi.Core.Modularity;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.Business.MapperProfile;

namespace TsiErp.Business
{
    [RelatedModules(typeof(TsiCachingModule))]
    public class TsiBusinessModule : TsiModule
    {
        List<Type> dependencies = new List<Type>();

        public TsiBusinessModule()
        {
            var dependencyDescriptors = this.GetType().GetCustomAttributes().OfType<IDependedTypesProvider>();

            foreach (var descriptor in dependencyDescriptors)
            {
                foreach (var dependedModuleType in descriptor.GetDependedTypes())
                {
                    dependencies.Add(dependedModuleType);
                }
            }
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            SetMapperToObjectMapper();

            foreach (var item in dependencies)
            {
                var instance = (TsiModule)Activator.CreateInstance(item);

                services.AddDependencyResolvers(new TsiModule[] 
                {
                    instance
                });
            }
        }

        static void SetMapperToObjectMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<TsiErpMapperProfile>();
            });

            ObjectMapper.Mapper = new Mapper(config);

        }
    }
}
