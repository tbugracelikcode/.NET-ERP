using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.Business.MapperProfile;
using Microsoft.AspNetCore.Http;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;

namespace TsiErp.Business
{

    public class TsiBusinessService 
    {
        public void RegisterService(IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

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
