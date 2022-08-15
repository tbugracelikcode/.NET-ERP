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

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory()).ConfigureContainer<ContainerBuilder>(container =>
{
    container.RegisterModule(new TsiBusinessModule());
});

builder.Services.AddDependencyResolvers(new ITsiCoreService[]
{
    new TsiBusinessService()
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
