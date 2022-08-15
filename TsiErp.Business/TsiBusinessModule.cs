using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.DynamicProxy;
using Autofac.Extras.DynamicProxy;
using TsiErp.Business.Entities.Branch.Services;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.Branch;
using TsiErp.Business.Entities.Period.Services;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.Period;
using Tsi.IoC.IoC.Autofac.Interceptors;
using Tsi.IoC.IoC.Autofac;

namespace TsiErp.Business
{
    public class TsiBusinessModule : TsiModule
    {
        protected override void Load(ContainerBuilder builder)
        {

            #region Business Serivice Registrations
            builder.RegisterType<BranchesAppService>().As<IBranchesAppService>();
            builder.RegisterType<EFCoreBranchesRepository>().As<IBranchesRepository>();

            builder.RegisterType<PeriodsAppService>().As<IPeriodsAppService>();
            builder.RegisterType<EFCorePeriodsRepository>().As<IPeriodsRepository>();
            #endregion


            var assembly = System.Reflection.Assembly.GetExecutingAssembly();

            builder.RegisterAssemblyTypes(assembly).AsImplementedInterfaces()
                .EnableInterfaceInterceptors(new ProxyGenerationOptions()
                {
                    Selector = new AspectInterceptorSelector()
                }).SingleInstance();
        }
    }
}
