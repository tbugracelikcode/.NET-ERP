using Blazored.Modal;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.LifecycleEvents;
using Syncfusion.Blazor;
using System.Reflection;
using TsiErp.Business;
using TsiErp.Business.Entities.GeneralSystemIdentifications.FicheNumber.Services;
using TsiErp.Business.Entities.User.Services;
using TsiErp.Business.Entities.WorkOrder.Services;
using TsiErp.UretimEkranUI.Services;
using TsiErp.UretimEkranUI.Utilities.ModalUtilities;
#if WINDOWS
using Microsoft.UI;
using Microsoft.UI.Windowing;
using Windows.Graphics;
#endif

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

            //            var builder = MauiApp.CreateBuilder();
            //            builder
            //                .UseMauiApp<App>()
            //                .ConfigureFonts(fonts =>
            //                {
            //                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            //                }).ConfigureLifecycleEvents(events =>

            //                {


            //#if WINDOWS

            //            events.AddWindows(wndLifeCycleBuilder =>

            //            {               

            //                wndLifeCycleBuilder.OnWindowCreated(window =>

            //                {

            //                    IntPtr nativeWindowHandle = WinRT.Interop.WindowNative.GetWindowHandle(window);

            //                    WindowId nativeWindowId = Win32Interop.GetWindowIdFromWindow(nativeWindowHandle);

            //                    AppWindow appWindow = AppWindow.GetFromWindowId(nativeWindowId);

            //                    var p = appWindow.Presenter as OverlappedPresenter;

            //                    window.ExtendsContentIntoTitleBar = false;

            //                    p.SetBorderAndTitleBar(false, false);

            //                });

            //            });

            //#endif

            //                }); 

            builder.Services.AddMauiBlazorWebView();

            builder.Services.AddLocalization();

            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1NHaF5cWWBCf1FpRmJGdld5fUVHYVZUTXxaS00DNHVRdkdgWH5ednRWQ2FZUk1xXUE=");

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Logging.AddDebug();
#endif

            builder.Services.AddBlazoredModal();
            builder.Services.AddSyncfusionBlazor();
            builder.Services.AddDevExpressBlazor();
            builder.Services.AddScoped<ModalManager>();
            builder.Services.AddScoped<AppService>();
            builder.Services.AddScoped<IFicheNumbersAppService, FicheNumbersAppService>();
            builder.Services.AddScoped<IUsersAppService, UsersAppService>();
            builder.Services.AddScoped<IWorkOrdersAppService, WorkOrdersAppService>();



            return builder.Build();
        }

    }
}