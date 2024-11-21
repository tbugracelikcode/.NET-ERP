using Autofac;
using Autofac.Extensions.DependencyInjection;
using Blazored.Modal;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Serialization;
using Syncfusion.Blazor;
using System.Globalization;
using System.Reflection;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TsiErp.Business;
using TsiErp.Business.DependencyResolvers.Autofac;
using TsiErp.DataAccess;
using TsiErp.ErpUI.Components.Commons.Spinner;
using TsiErp.ErpUI.Services.Dashboard;
using TsiErp.ErpUI.Services.Dashboard.OperationalDashboard.OpenOrderAnalysis;
using TsiErp.ErpUI.Shared;
using TsiErp.ErpUI.Utilities.ModalUtilities;


var builder = WebApplication.CreateBuilder(args);




//builder.Services.AddTransient<ApplicationService>();

ConfigureBusiness(builder);

ConfigureDataAccess(builder);

ConfigureErpUI(builder);

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory()).ConfigureContainer<ContainerBuilder>(container =>
{
    container.RegisterModule(new AutofacBusinessModule());
});


builder.Services.AddSyncfusionBlazor();
builder.Services.AddDevExpressBlazor(configure => configure.BootstrapVersion = DevExpress.Blazor.BootstrapVersion.v5);

builder.Services.AddDevExpressServerSideBlazorReportViewer();

builder.Services.AddLocalization();

builder.Services.AddSingleton(typeof(ISyncfusionStringLocalizer), typeof(SyncfusionLocalizer));
builder.Services.AddScoped(typeof(IDashboardAppServices), typeof(DashboardAppServices));
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    // Define the list of cultures your app will support
    var supportedCultures = new List<CultureInfo>()
    {
                    new CultureInfo("en-US"),
                    //new CultureInfo("de"),
                    //new CultureInfo("fr"),
                    //new CultureInfo("ar"),
                    //new CultureInfo("zh"),
                    new CultureInfo("tr"),
    };

    // Set the default culture
    options.DefaultRequestCulture = new RequestCulture("tr");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;

});




builder.Services.AddRazorPages();
builder.Services.AddControllers().AddNewtonsoftJson(options => { options.SerializerSettings.ContractResolver = new DefaultContractResolver(); });
builder.Services.AddSignalR(e => { e.MaximumReceiveMessageSize = 102400000; });
builder.Services.AddSignalR(e => { e.MaximumReceiveMessageSize = 102400000; });
builder.Services.AddServerSideBlazor().AddHubOptions(o =>
            {
                o.MaximumReceiveMessageSize = 102400000;
            });


builder.Services.AddBlazoredModal();
builder.Services.AddDevExpressBlazor();
builder.Services.AddScoped<ModalManager>();
builder.Services.AddScoped<SpinnerService>();
builder.Services.AddScoped<IOpenOrderAnalysisAppService, OpenOrderAnalysisAppService>();


var app = builder.Build();



Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("NzA0MDk0QDMyMzAyZTMyMmUzMEVjb29PTkxlM3YvRVZwVTR5U0VCT2toK24vMEJlYmFVeFkwRlYrT1cwMzA9");

// Configure the HTTP request pipeline.
app.UseRequestLocalization(app.Services.GetService<IOptions<RequestLocalizationOptions>>().Value);
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();


app.UseRouting();
app.UseCors();

app.MapDefaultControllerRoute();
app.MapControllers();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();


static void ConfigureBusiness(WebApplicationBuilder builder)
{
    builder.Services.RegisterDependencies(Assembly.Load("TsiErp.Business"));
   
    var instance = (TsiBusinessModule)Activator.CreateInstance(typeof(TsiBusinessModule));

    instance.ConfigureServices(builder.Services);
}

static void ConfigureDataAccess(WebApplicationBuilder builder)
{
    builder.Services.RegisterDependencies(Assembly.Load("TsiErp.DataAccess"));

    var instance = (TsiDataAccessModule)Activator.CreateInstance(typeof(TsiDataAccessModule));

    instance.ConfigureServices(builder.Services);
}

static void ConfigureErpUI(WebApplicationBuilder builder)
{
    builder.Services.RegisterDependencies(Assembly.Load("TsiErp.ErpUI"));

    var instance = (TsiBusinessModule)Activator.CreateInstance(typeof(TsiBusinessModule));

    instance.ConfigureServices(builder.Services);
}