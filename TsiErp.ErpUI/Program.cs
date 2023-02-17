using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Syncfusion.Blazor;
using System.Globalization;
using TsiErp.ErpUI.Shared;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Localization;
 using Newtonsoft.Json.Serialization;
using Autofac.Extensions.DependencyInjection;
using TsiErp.Business.DependencyResolvers.Autofac;
using Autofac;
using System.Reflection;
using TsiErp.Business;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TsiErp.Business.Entities.Branch.Services;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.Branch;
using TsiErp.DataAccess.EntityFrameworkCore;
using Blazored.Modal;
using Blazored.Modal.Services;
using TsiErp.ErpUI.Utilities.ModalUtilities;
using Microsoft.EntityFrameworkCore;
using Tsi.Core.Services.BusinessCoreServices;
using TsiErp.ErpUI.Services;
using TsiErp.DataAccess;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContextFactory<TsiErpDbContext>(
        options =>
            options.UseSqlServer(@"Server=94.73.145.4;Database=u0364806_TSIERP;UID=u0364806_TSIERP;PWD=u=xfJ@i-7H5-VN23;"), ServiceLifetime.Transient);

builder.Services.AddTransient<ApplicationService>();

ConfigureBusiness(builder);

ConfigureDataAccess(builder);

ConfigureErpUI(builder);

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory()).ConfigureContainer<ContainerBuilder>(container =>
{
    container.RegisterModule(new AutofacBusinessModule());
});

builder.Services.AddSyncfusionBlazor();
builder.Services.AddDevExpressBlazor();


builder.Services.AddSingleton(typeof(ISyncfusionStringLocalizer), typeof(SyncfusionLocalizer));
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
                // Define the list of cultures your app will support
    var supportedCultures = new List<CultureInfo>()
    {
                    new CultureInfo("en-US"),
                    new CultureInfo("de"),
                    new CultureInfo("fr"),
                    new CultureInfo("ar"),
                    new CultureInfo("zh"),
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
builder.Services.AddServerSideBlazor().AddHubOptions(o=>
            {
                o.MaximumReceiveMessageSize=102400000;
            });


builder.Services.AddBlazoredModal();
builder.Services.AddDevExpressBlazor();
builder.Services.AddScoped<ModalManager>();


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