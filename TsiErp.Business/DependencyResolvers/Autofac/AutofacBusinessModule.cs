using Autofac;
using Autofac.Extras.DynamicProxy;
using Castle.DynamicProxy;
using Tsi.Core.Utilities.Interceptors;
using TsiErp.Business.Entities.Authentication.RolePermissions;


namespace TsiErp.Business.DependencyResolvers.Autofac
{
    public class AutofacBusinessModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {

            var assembly = System.Reflection.Assembly.GetExecutingAssembly();

            builder.RegisterAssemblyTypes(assembly).AsImplementedInterfaces()
                .EnableInterfaceInterceptors(new ProxyGenerationOptions()
                {
                    Selector = new AspectInterceptorSelector()
                }).SingleInstance();
        }
    }
}
