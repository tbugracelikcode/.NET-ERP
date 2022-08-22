using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tsi.Core.Modularity
{
    public interface ITsiModule
    {
        void ConfigureServices(IServiceCollection services);
    }
}
