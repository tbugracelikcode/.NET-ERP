using Autofac;
using Autofac.Extensions.DependencyInjection;
using Tsi.Logging.EntityFrameworkCore.Repositories;
using TsiErp.Business;
using TsiErp.Business.DependencyResolvers.Autofac;
using TsiErp.DataAccess.EntityFrameworkCore;
using Tsi.Core.Modularity.Extension;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddDbContext<TsiErpDbContext>();
builder.Services.AddDbContext<LogDbContext>();



builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory()).ConfigureContainer<ContainerBuilder>(container =>
{
    container.RegisterModule(new AutofacBusinessModule());
});

//builder.Services.AddDependencyResolvers(new TsiModule[]
//{
//    new TsiBusinessModule()
//});

ConfigureBusiness(builder);
ConfigureDataAccess(builder);
ConfigureLogging(builder);

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

static void ConfigureLogging(WebApplicationBuilder builder)
{
    builder.Services.RegisterDependencies(Assembly.Load("Tsi.Logging"));
    builder.Services.RegisterDependencies(Assembly.Load("Tsi.Logging.EntityFrameworkCore"));
}