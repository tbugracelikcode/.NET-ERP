using Microsoft.EntityFrameworkCore;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TsiErp.Business;
using Tsi.IoC.IoC.Autofac.Extensions;
using Tsi.IoC.IoC.Autofac;
using TsiErp.DataAccess.EntityFrameworkCore;
using TsiErp.Business.Entities.Branch;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.Branch;
using Tsi.Guids;
using TsiErp.DataAccess;
using TsiErp.Business.DependencyResolvers.Autofac;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


//var connectionString = builder.Configuration.GetConnectionString("Default");
//builder.Services.AddDbContext<TsiErpDbContext>(x => x.UseSqlServer(connectionString));

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory()).ConfigureContainer<ContainerBuilder>(container =>
{
    container.RegisterModule(new AutofacBusinessModule());
});

builder.Services.AddDependencyResolvers(new ITsiCoreModule[]
{
    new TsiBusinessCoreModule(),
    new TsiGuidsCoreModule()
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
