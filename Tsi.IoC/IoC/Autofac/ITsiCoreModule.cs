using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tsi.IoC.IoC.Autofac
{
    public interface ITsiCoreModule
    {
        void Load(IServiceCollection services);
    }
}
