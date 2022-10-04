using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Syncfusion.Blazor;
using System.Globalization;
using TsiErp.DashboardUI.Shared;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Localization;
 using Newtonsoft.Json.Serialization;
using TsiErp.DashboardUI.Data;
using TsiErp.DashboardUI.Services;
using TsiErp.DashboardUI.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IIstasyonOEEService,IstasyonOEEService>();
builder.Services.AddScoped< IPersonelService, PersonelService >();
builder.Services.AddScoped<IPersonelDetayService, PersonelDetayService>();
builder.Services.AddScoped<IStokDetayService, StokDetayService>();
builder.Services.AddScoped<IStokService, StokService>();
builder.Services.AddScoped<IIstasyonService, IstasyonService>();
builder.Services.AddScoped<IIstasyonDetayService, IstasyonDetayService>();
builder.Services.AddScoped<IUretimUygunsuzlukService, UretimUygunsuzlukService>();
builder.Services.AddScoped<IUretimUygunsuzlukDetayService,UretimUygunsuzlukDetayService>();
builder.Services.AddScoped<IFasonUygunsuzlukService, FasonUygunsuzlukService>();
builder.Services.AddScoped<IFasonUygunsuzlukDetayService, FasonUygunsuzlukDetayService>();
builder.Services.AddScoped<ITedarikciUygunsuzlukService, TedarikciUygunsuzlukService>();
builder.Services.AddScoped<ITedarikciUygunsuzlukDetayService, TedarikciUygunsuzlukDetayService>();
builder.Services.AddScoped<IGenelOEEService, GenelOEEService>();

// Add services to the container.
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
            builder.Services.AddSingleton<PowerPointService>();
            builder.Services.AddSingleton<WordService>();
            builder.Services.AddSingleton<PdfService>();
            builder.Services.AddSingleton<ExcelService>();
builder.Services.AddRazorPages();
            builder.Services.AddControllers().AddNewtonsoftJson(options => { options.SerializerSettings.ContractResolver = new DefaultContractResolver(); });
            builder.Services.AddSignalR(e => { e.MaximumReceiveMessageSize = 102400000; });
            builder.Services.AddSignalR(e => { e.MaximumReceiveMessageSize = 102400000; });
builder.Services.AddServerSideBlazor().AddHubOptions(o=>
            {
                o.MaximumReceiveMessageSize=102400000;
            });
builder.Services.AddSingleton<WeatherForecastService>();

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
