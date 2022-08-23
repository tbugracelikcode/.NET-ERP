using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;
using Tsi.Core.Modularity;

namespace TsiErp.Shared
{
    public class TsiSharedModule : TsiModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddLocalization();

            var supportedCultures = new List<CultureInfo> { new CultureInfo("en"), new CultureInfo("tr") };

            services.Configure<RequestLocalizationOptions>(options =>
            {
                options.DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture("en");
                options.SupportedUICultures = supportedCultures;
            });
        }
    }
}
