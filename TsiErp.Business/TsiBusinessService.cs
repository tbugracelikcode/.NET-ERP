using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Tsi.Caching;
using Tsi.Caching.Microsoft;
using Tsi.Guids;
using Tsi.IoC.IoC.Autofac;
using Tsi.Logging.EntityFrameworkCore.Repositories.EntityFrameworkCore;
using Tsi.Logging.Tsi.Services;
using TsiErp.Business.Entities.Branch.Services;
using TsiErp.Business.Entities.Period.Services;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.Business.MapperProfile;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.Branch;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.Period;

namespace TsiErp.Business
{

    public class TsiBusinessService : ITsiCoreService
    {
        public void RegisterService(IServiceCollection services)
        {
            services.AddMemoryCache();
            services.AddSingleton<ICacheManager, MemoryCacheManager>();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddSingleton<IGuidGenerator, SequentialGuidGenerator>();

            services.AddScoped<ILogsAppService, LogsAppService>();
            services.AddScoped<ILogsRepository, EfCoreLogsRepository>();

            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            SetMapperToObjectMapper();
        }

        private void SetMapperToObjectMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<TsiErpMapperProfile>();
            });

            ObjectMapper.Mapper = new Mapper(config);

        }
    }
}
