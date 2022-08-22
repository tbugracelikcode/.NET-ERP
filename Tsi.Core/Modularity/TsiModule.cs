using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tsi.Core.Modularity
{
    public abstract class TsiModule : ITsiModule
    {
        public virtual void ConfigureServices(IServiceCollection services)
        {
        }
    }
}
