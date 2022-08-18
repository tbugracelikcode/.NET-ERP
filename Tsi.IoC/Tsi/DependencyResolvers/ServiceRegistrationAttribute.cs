using System;
using System.Collections.Generic;
using System.Text;

namespace Tsi.IoC.Tsi.DependencyResolvers
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ServiceRegistrationAttribute : Attribute
    {
        public Type ImplementedInterface { get; }
        public DependencyInjectionType Scope { get; }

        public ServiceRegistrationAttribute(Type implementedInterface, DependencyInjectionType scope = DependencyInjectionType.Scoped)
        {
            ImplementedInterface = implementedInterface;
            Scope = scope;
        }
    }
}
