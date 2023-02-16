using Autofac;
using Autofac.Extensions.DependencyInjection;
using TsiErp.Business;
using TsiErp.Business.DependencyResolvers.Autofac;
using TsiErp.DataAccess.EntityFrameworkCore;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using System.Reflection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using TsiErp.Business.Entities.Branch.Services;
using Microsoft.Extensions.DependencyInjection;
using Tsi.Core.Services.BusinessCoreServices;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddControllers().AddControllersAsServices();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddDbContextFactory<TsiErpDbContext>();

ConfigureBusiness(builder);
ConfigureDataAccess(builder);

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory()).ConfigureContainer<ContainerBuilder>(container =>
{
    container.RegisterModule(new AutofacBusinessModule());
});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();


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
}