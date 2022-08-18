using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Tsi.IoC.IoC.Autofac;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.Business.MapperProfile;
using Tsi.IoC.Tsi.DependencyResolvers;
using Microsoft.AspNetCore.Http;

namespace TsiErp.Business
{

    public class TsiBusinessService : ITsiCoreService
    {
        public void RegisterService(IServiceCollection services)
        {
            services.AddMemoryCache();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();


            services.RegisterDependencies(Assembly.Load("Tsi.Guids"));
            services.RegisterDependencies(Assembly.Load("Tsi.Caching"));
            services.RegisterDependencies(Assembly.Load("TsiErp.Business"));
            services.RegisterDependencies(Assembly.Load("TsiErp.DataAccess"));
            services.RegisterDependencies(Assembly.Load("Tsi.Logging"));
            services.RegisterDependencies(Assembly.Load("Tsi.Logging.EntityFrameworkCore"));

            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            SetMapperToObjectMapper();
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
