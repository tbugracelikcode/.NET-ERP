using Blazored.Modal;
using Microsoft.Extensions.Logging;
using Syncfusion.Blazor;
using TsiErp.UretimEkranUI.Utilities.ModalUtilities;

namespace TsiErp.UretimEkranUI
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddMauiBlazorWebView();

            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("NzA0MDk0QDMyMzAyZTMyMmUzMEVjb29PTkxlM3YvRVZwVTR5U0VCT2toK24vMEJlYmFVeFkwRlYrT1cwMzA9");

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Logging.AddDebug();
#endif

            builder.Services.AddBlazoredModal();
            builder.Services.AddSyncfusionBlazor();
            builder.Services.AddDevExpressBlazor();
            builder.Services.AddScoped<ModalManager>();

            return builder.Build();
        }
    }
}