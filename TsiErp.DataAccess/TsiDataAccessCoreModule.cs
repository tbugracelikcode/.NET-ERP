using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.IoC.IoC.Autofac;
using TsiErp.DataAccess.EntityFrameworkCore;

namespace TsiErp.DataAccess
{
    public class TsiDataAccessCoreModule : ITsiCoreModule
    {
        public void Load(IServiceCollection services)
        {
            services.AddDbContext<TsiErpDbContext>(options =>
                {
                    options.UseQueryTrackingBehavior(Microsoft.EntityFrameworkCore.QueryTrackingBehavior.NoTracking);
                });
        }
    }
}
