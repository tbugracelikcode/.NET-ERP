using AutoMapper;
using AutoMapper.EquivalencyExpression;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Tsi.Core.CrossCuttingConcerns.Caching;
using Tsi.Core.Modularity;
using Tsi.Core.Modularity.Extension;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.Business.MapperProfile;

namespace TsiErp.Business
{
    [RelatedModules
        (
        typeof(TsiCachingModule)
        )]
    public class TsiBusinessModule : TsiModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddScoped<CookieServices>();

            services.AddLocalization();

            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            SetMapperToObjectMapper();

            services.RegisterRelatedModuleAssemblies(typeof(TsiBusinessModule));

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
