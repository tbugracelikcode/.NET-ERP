using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Tsi.Guids;
using Tsi.IoC.IoC.Autofac;

namespace TsiErp.Business
{
    public class TsiGuidsCoreModule : ITsiCoreModule
    {
        public void Load(IServiceCollection services)
        {
            services.AddSingleton<IGuidGenerator, SequentialGuidGenerator>();
        }
    }
}
