using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Tsi.Core.CrossCuttingConcerns.Caching.Microsoft;
using Tsi.Core.Modularity;

namespace Tsi.Core.CrossCuttingConcerns.Caching
{
    public class TsiCachingModule : TsiModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddMemoryCache();
            services.AddSingleton<ICacheManager,MemoryCacheManager>();
        }
    }
}
