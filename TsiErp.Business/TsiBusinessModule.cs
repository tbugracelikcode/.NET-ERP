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
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using Tsi.Core.Modularity.Extension;

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
