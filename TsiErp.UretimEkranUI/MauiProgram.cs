using Blazored.Modal;
using Microsoft.Extensions.Logging;
using Syncfusion.Blazor;
using System.Reflection;
using TsiErp.Business;
using TsiErp.Business.Entities.Employee.Services;
using TsiErp.Business.Entities.FirstProductApproval.Services;
using TsiErp.Business.Entities.GeneralSystemIdentifications.FicheNumber.Services;
using TsiErp.Business.Entities.GeneralSystemIdentifications.UserPermission.Services;
using TsiErp.Business.Entities.HaltReason.Services;
using TsiErp.Business.Entities.Menu.Services;
using TsiErp.Business.Entities.OperationalQualityPlan.Services;
using TsiErp.Business.Entities.Other.GetSQLDate.Services;
using TsiErp.Business.Entities.ProductionManagement.OperationAdjustment.Services;
using TsiErp.Business.Entities.QualityControl.OperationalQualityPlan.Services;
using TsiErp.Business.Entities.User.Services;
using TsiErp.Business.Entities.WorkOrder.Services;
using TsiErp.Connector.Services;
using TsiErp.UretimEkranUI.Services;
using TsiErp.UretimEkranUI.Utilities.ModalUtilities;
using TsiErp.UretimEkranUI.Utilities.NavigationUtilities;
using TsiErp.Business.Entities.Station.Services;
using TsiErp.Business.Entities.OperationUnsuitabilityReport.Services;
using TsiErp.Business.Entities.GeneralSystemIdentifications.NotificationTemplate.Services;
using TsiErp.Business.Entities.Other.Notification.Services;
#if WINDOWS
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
            //builder.Services.AddDevExpressBlazor();
            builder.Services.AddDevExpressBlazor(configure => configure.BootstrapVersion = DevExpress.Blazor.BootstrapVersion.v5);
            //builder.Services.AddSingleton<LocalDbService>();
            builder.Services.AddSingleton<OperationDetailLocalDbService>();
            builder.Services.AddSingleton<ParametersTableLocalDbService>();
            builder.Services.AddSingleton<OperationHaltReasonsTableLocalDbService>();
            builder.Services.AddSingleton<LoggedUserLocalDbService>();
            builder.Services.AddSingleton<ScrapLocalDbService>();
            builder.Services.AddSingleton<OperationAdjustmentLocalDbService>();
            builder.Services.AddSingleton<SystemGeneralStatusLocalDbService>();
            builder.Services.AddScoped<ModalManager>();
            builder.Services.AddScoped<Navigation>();
            builder.Services.AddScoped<AppService>();
            builder.Services.AddScoped<ConnectorService>();
            builder.Services.AddScoped<NavMenuService>();
            builder.Services.AddScoped<OperationDetailPageService>();
            builder.Services.AddScoped<IFicheNumbersAppService, FicheNumbersAppService>();
            builder.Services.AddScoped<IUsersAppService, UsersAppService>();
            builder.Services.AddScoped<IWorkOrdersAppService, WorkOrdersAppService>();
            builder.Services.AddScoped<IEmployeesAppService, EmployeesAppService>();
            builder.Services.AddScoped<IFirstProductApprovalsAppService, FirstProductApprovalsAppService>();
            builder.Services.AddScoped<IFicheNumbersAppService, FicheNumbersAppService>();
            builder.Services.AddScoped<IOperationalQualityPlansAppService, OperationalQualityPlansAppService>();
            builder.Services.AddScoped<IOperationAdjustmentsAppService, OperationAdjustmentsAppService>();
            builder.Services.AddScoped<IUserPermissionsAppService, UserPermissionsAppService>();
            builder.Services.AddScoped<IMenusAppService, MenusAppService>();
            builder.Services.AddScoped<IHaltReasonsAppService, HaltReasonsAppService>();
            builder.Services.AddScoped<IGetSQLDateAppService, GetSQLDateAppService>();
            builder.Services.AddScoped<IProtocolServices, ProtocolServices>();
            builder.Services.AddScoped<IStationsAppService, StationsAppService>();
            builder.Services.AddScoped<IOperationUnsuitabilityReportsAppService, OperationUnsuitabilityReportsAppService>();
            builder.Services.AddScoped<INotificationTemplatesAppService, NotificationTemplatesAppService>();
            builder.Services.AddScoped<INotificationsAppService, NotificationsAppService>();


            ConfigureBusiness(builder);


            return builder.Build();
        }

        static void ConfigureBusiness(MauiAppBuilder builder)
        {
            var instance = (TsiBusinessModule)Activator.CreateInstance(typeof(TsiBusinessModule));

            instance.ConfigureServices(builder.Services);
        }

    }


}