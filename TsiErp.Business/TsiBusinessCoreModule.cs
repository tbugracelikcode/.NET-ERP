using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Tsi.IoC.IoC.Autofac;
using TsiErp.Business.Entities.Branch.Services;
using TsiErp.Business.Entities.Period.Services;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.Business.MapperProfile;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.Branch;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.Period;

namespace TsiErp.Business
{

    public class TsiBusinessCoreModule : ITsiCoreModule
    {
        public void Load(IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

           

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
