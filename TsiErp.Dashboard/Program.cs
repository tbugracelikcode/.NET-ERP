
using Blazor.AdminLte;
using TsiErp.Dashboard.Services;
using TsiErp.Dashboard.Data;
using DevExpress.Blazor;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddAdminLte();

builder.Services.AddScoped<IstasyonService>();
builder.Services.AddSingleton<WeatherForecastService>();
builder.Services.AddSingleton<wet1Service>();
builder.Services.AddDevExpressBlazor(opt => 
{ 
    opt.BootstrapVersion = BootstrapVersion.v5; 
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}



app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
