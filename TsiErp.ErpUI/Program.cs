﻿using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Syncfusion.Blazor;
using System.Globalization;
using TsiErp.ErpUI.Shared;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Localization;
 using Newtonsoft.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddSingleton<HttpClient>();

builder.Services.AddSyncfusionBlazor();
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
                };
                    // Set the default culture
                    options.DefaultRequestCulture = new RequestCulture("en-US");
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

builder.Services.AddDevExpressBlazor();

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
